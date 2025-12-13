import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';


export function RegisterPage() {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();


  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };


  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');


    if (formData.password !== formData.confirmPassword) {
      setError('Passwords do not match');
      return;
    }


    setIsLoading(true);


    try {
      const response = await fetch('http://localhost:5001/api/auth/register', {
        method: 'POST',
        headers: { 
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ 
          username: formData.username, 
          email: formData.email, 
          password: formData.password 
        })
      });


      const data = await response.json();
      console.log('🔍 REGISTER API RESPONSE:', data);


      if (!response.ok) {
        console.log('❌ Register error:', data);
        setError(data.message || data.error || 'Registration failed');
        return;
      }


      // ✅ НОВАЯ СТРУКТУРА ОТВЕТА
      const token = data.accessToken;
      const userData = data.user;


      if (!token || !userData) {
        console.error('❌ Missing token or user:', data);
        setError('Invalid response from server');
        return;
      }


      console.log('✅ REGISTER SUCCESS:', { token: 'OK', user: userData });
      
      // ✅ СОХРАНЯЕМ ДАННЫЕ В ЛОКАЛЬНОЕ ХРАНИЛИЩЕ
      localStorage.setItem('token', token);
      localStorage.setItem('user', JSON.stringify(userData));
      
      // ✅ ТРИГГЕРИМ СОБЫТИЕ ДЛЯ HEADER
      window.dispatchEvent(new Event('userUpdated'));
      
      console.log('💾 Saved to localStorage:');
      console.log('  token:', localStorage.getItem('token') ? 'OK' : 'MISSING');
      console.log('  user:', localStorage.getItem('user') ? 'OK' : 'MISSING');
      
      // ✅ НАВИГИРУЕМ СРАЗУ БЕЗ ЗАДЕРЖКИ
      navigate('/');
    } catch (err) {
      console.error('🌐 Network error:', err);
      setError('Network error. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };


  return (
    <div style={{ padding: '50px', maxWidth: '400px', margin: '0 auto' }}>
      <h1 style={{ textAlign: 'center', marginBottom: '30px' }}>Register</h1>
      
      {error && (
        <div style={{ 
          background: '#fee', 
          color: '#c33', 
          padding: '15px', 
          borderRadius: '5px', 
          marginBottom: '20px',
          borderLeft: '4px solid #c33'
        }}>
          {error}
        </div>
      )}


      <form onSubmit={handleSubmit}>
        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Username</label>
          <input
            name="username"
            type="text"
            value={formData.username}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: '10px', border: '1px solid #ddd', borderRadius: '4px' }}
          />
        </div>


        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Email</label>
          <input
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: '10px', border: '1px solid #ddd', borderRadius: '4px' }}
          />
        </div>


        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Password</label>
          <input
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: '10px', border: '1px solid #ddd', borderRadius: '4px' }}
          />
        </div>


        <div style={{ marginBottom: '20px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Confirm Password</label>
          <input
            name="confirmPassword"
            type="password"
            value={formData.confirmPassword}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: '10px', border: '1px solid #ddd', borderRadius: '4px' }}
          />
        </div>


        <button 
          type="submit" 
          disabled={isLoading}
          style={{ 
            width: '100%', 
            padding: '12px', 
            background: isLoading ? '#ccc' : '#208092', 
            color: 'white', 
            border: 'none', 
            borderRadius: '4px',
            cursor: isLoading ? 'not-allowed' : 'pointer',
            fontSize: '16px'
          }}
        >
          {isLoading ? 'Registering...' : 'Register'}
        </button>
      </form>


      <p style={{ textAlign: 'center', marginTop: '20px' }}>
        Already have an account?{' '}
        <Link to="/login" style={{ color: '#208092', textDecoration: 'none' }}>
          Login
        </Link>
      </p>
    </div>
  );
}
