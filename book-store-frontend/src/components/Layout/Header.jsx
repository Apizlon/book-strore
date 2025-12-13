import { Link, useNavigate } from 'react-router-dom';
import { ShoppingCart, LogOut } from 'lucide-react';

export function Header({ user }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    console.log('🚪 Logging out...');
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    navigate('/');
    window.location.reload();
  };

  console.log('🧑 Header render: user =', user ? user.username : 'null');

  return (
    <header style={{ background: '#fff', padding: '20px', boxShadow: '0 2px 4px rgba(0,0,0,0.1)' }}>
      <div style={{ maxWidth: '1200px', margin: '0 auto', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Link to="/" style={{ fontSize: '24px', fontWeight: 'bold', color: '#208092', textDecoration: 'none' }}>
          📚 BookStore
        </Link>

        <nav style={{ display: 'flex', gap: '20px' }}>
          <Link to="/books" style={{ color: '#666', textDecoration: 'none' }}>Books</Link>
          {user && (
            <>
              <Link to="/profile" style={{ color: '#666', textDecoration: 'none' }}>Profile</Link>
              <Link to="/orders" style={{ color: '#666', textDecoration: 'none' }}>Orders</Link>
            </>
          )}
          {user?.role === 'Moderator' || user?.role === 'Admin' ? (
            <Link to="/admin" style={{ color: '#666', textDecoration: 'none' }}>Admin</Link>
          ) : null}
        </nav>

        <div style={{ display: 'flex', alignItems: 'center', gap: '20px' }}>
          <Link to="/cart" style={{ display: 'flex', alignItems: 'center', gap: '5px', textDecoration: 'none' }}>
            <ShoppingCart style={{ width: '24px', height: '24px', color: '#666' }} />
          </Link>

          {user ? (
            <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
              <span style={{ fontWeight: '500', color: '#333' }}>Hi, {user.username}</span>
              <button onClick={handleLogout} style={{ background: 'none', border: 'none', cursor: 'pointer' }}>
                <LogOut style={{ width: '20px', height: '20px', color: '#dc2626' }} />
              </button>
            </div>
          ) : (
            <>
              <Link to="/login" style={{ color: '#208092', textDecoration: 'none' }}>Login</Link>
              <Link 
                to="/register" 
                style={{ 
                  background: '#208092', 
                  color: 'white', 
                  padding: '8px 16px', 
                  borderRadius: '4px', 
                  textDecoration: 'none',
                  fontWeight: '500'
                }}
              >
                Register
              </Link>
            </>
          )}
        </div>
      </div>
    </header>
  );
}
