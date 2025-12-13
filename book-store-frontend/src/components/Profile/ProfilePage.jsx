import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

export function ProfilePage() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  // ✅ ФУНКЦИИ ДЛЯ ПАРСИНГА
  const parseDate = (dateString) => {
    if (!dateString) return 'Not set';
    try {
      return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    } catch {
      return 'Invalid date';
    }
  };

  const getRoleName = (roleNumber) => {
    const roleMap = {
      0: 'User',
      1: 'Moderator', 
      2: 'Admin'
    };
    return roleMap[roleNumber] || `Unknown (${roleNumber})`;
  };

  useEffect(() => {
    fetchProfile();
  }, []);

  const fetchProfile = async () => {
    try {
      const token = localStorage.getItem('token');
      const storedUser = localStorage.getItem('user');
      
      if (!token || !storedUser) {
        throw new Error('No authentication data');
      }

      const parsedStoredUser = JSON.parse(storedUser);
      const userId = parsedStoredUser.id;

      console.log('🔍 Fetching profile for userId:', userId);

      const response = await fetch(`http://localhost:5002/api/users/${userId}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        console.error('❌ Profile API error:', response.status, errorData);
        throw new Error(errorData.message || `HTTP ${response.status}`);
      }

      const data = await response.json();
      console.log('✅ Profile loaded:', data);
      setUser(data);
    } catch (err) {
      console.error('❌ Profile fetch failed:', err);
      setError(err.message || 'Failed to load profile');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div style={{ padding: '50px', textAlign: 'center', fontSize: '18px' }}>
        Loading profile...
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ padding: '50px', textAlign: 'center', color: 'red' }}>
        <h2>Error</h2>
        <p>{error}</p>
        <Link 
          to="/login" 
          style={{ 
            background: '#dc2626', 
            color: 'white', 
            padding: '12px 24px', 
            borderRadius: '8px', 
            textDecoration: 'none',
            display: 'inline-block',
            marginTop: '20px'
          }}
        >
          Go to Login
        </Link>
      </div>
    );
  }

  return (
    <div style={{ padding: '50px', maxWidth: '800px', margin: '0 auto' }}>
      <h1 style={{ fontSize: '32px', marginBottom: '30px' }}>My Profile</h1>
      
      <div style={{ 
        background: '#f9f9f9', 
        padding: '32px', 
        borderRadius: '12px', 
        border: '1px solid #eee',
        marginBottom: '30px'
      }}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '30px', maxWidth: '600px' }}>
          <div>
            <p><strong>ID:</strong> {user.id}</p>
            <p><strong>Username:</strong> {user.username}</p>
            <p><strong>Email:</strong> {user.email || 'Not set'}</p>
            <p><strong>Role:</strong> 
              <span style={{ 
                padding: '6px 16px', 
                borderRadius: '20px', 
                background: user.role === 2 ? '#fef2f2' : user.role === 1 ? '#fef3c7' : '#f0fdf4',
                color: user.role === 2 ? '#dc2626' : user.role === 1 ? '#d97706' : '#16a34a',
                marginLeft: '8px',
                fontWeight: '600',
                fontSize: '14px'
              }}>
                {getRoleName(user.role)}
              </span>
            </p>
            <p><strong>Date of Birth:</strong> {parseDate(user.dateOfBirth)}</p>
            <p><strong>Status:</strong> 
              <span style={{ 
                padding: '6px 16px', 
                borderRadius: '20px', 
                background: user.isActive ? '#f0fdf4' : '#fef2f2',
                color: user.isActive ? '#16a34a' : '#dc2626',
                fontWeight: '600',
                fontSize: '14px'
              }}>
                {user.isActive ? '✅ Active' : '❌ Inactive'}
              </span>
            </p>
          </div>
          <div>
            <p><strong>Registration Date:</strong> {parseDate(user.registrationDate)}</p>
            <p><strong>Last Updated:</strong> {parseDate(user.updatedAt)}</p>
            {user.permissions && user.permissions.length > 0 && (
              <div>
                <p style={{ marginBottom: '12px', fontWeight: '600' }}><strong>Permissions ({user.permissions.length}):</strong></p>
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                  {user.permissions.map((perm, index) => (
                    <span key={index} style={{ 
                      padding: '6px 12px', 
                      background: '#e0f2fe', 
                      color: '#0369a1',
                      borderRadius: '6px',
                      fontSize: '13px',
                      fontWeight: '500',
                      border: '1px solid #bae6fd'
                    }}>
                      {perm}
                    </span>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
      </div>

      <div style={{ display: 'flex', gap: '20px', flexWrap: 'wrap', justifyContent: 'center' }}>
        <Link 
          to="/orders" 
          style={{ 
            background: '#208092', 
            color: 'white', 
            padding: '14px 28px', 
            borderRadius: '8px', 
            textDecoration: 'none',
            fontWeight: '600',
            fontSize: '16px',
            boxShadow: '0 4px 12px rgba(32,128,146,0.3)'
          }}
        >
          📦 My Orders
        </Link>
        <Link 
          to="/cart" 
          style={{ 
            background: '#6b7280', 
            color: 'white', 
            padding: '14px 28px', 
            borderRadius: '8px', 
            textDecoration: 'none',
            fontWeight: '600',
            fontSize: '16px',
            boxShadow: '0 4px 12px rgba(107,114,128,0.3)'
          }}
        >
          🛒 My Cart
        </Link>
        <Link 
          to="/books" 
          style={{ 
            background: '#3b82f6', 
            color: 'white', 
            padding: '14px 28px', 
            borderRadius: '8px', 
            textDecoration: 'none',
            fontWeight: '600',
            fontSize: '16px',
            boxShadow: '0 4px 12px rgba(59,130,246,0.3)'
          }}
        >
          📚 Continue Shopping
        </Link>
      </div>
    </div>
  );
}
