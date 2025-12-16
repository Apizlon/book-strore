import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { LogOut, ShoppingCart, Home, User } from 'lucide-react';


export function Header() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();


  useEffect(() => {
    const loadUser = () => {
      const storedUser = localStorage.getItem('user');
      console.log('🧑 Header useEffect: storedUser =', storedUser);
      
      if (storedUser) {
        try {
          const parsedUser = JSON.parse(storedUser);
          console.log('🧑 Header parsed user:', parsedUser);
          setUser(parsedUser);
        } catch (e) {
          console.error('Failed to parse user:', e);
          setUser(null);
        }
      } else {
        setUser(null);
      }
    };


    loadUser();


    // ✅ СЛУШАЕМ КАСТОМНОЕ СОБЫТИЕ
    window.addEventListener('userUpdated', loadUser);
    window.addEventListener('storage', loadUser);
    
    return () => {
      window.removeEventListener('userUpdated', loadUser);
      window.removeEventListener('storage', loadUser);
    };
  }, []);


  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
    navigate('/login');
  };


  return (
    <header style={{
      background: '#fff',
      borderBottom: '1px solid #e5e7eb',
      padding: '16px 20px',
      position: 'sticky',
      top: 0,
      zIndex: 50
    }}>
      <div style={{
        maxWidth: '1200px',
        margin: '0 auto',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center'
      }}>
        {/* Logo */}
        <Link 
          to="/" 
          style={{
            fontSize: '20px',
            fontWeight: 'bold',
            color: '#208092',
            textDecoration: 'none',
            display: 'flex',
            alignItems: 'center',
            gap: '8px'
          }}
        >
          <Home style={{ width: '24px', height: '24px' }} />
          BookStore
        </Link>


        {/* Navigation */}
        <nav style={{
          display: 'flex',
          gap: '20px',
          alignItems: 'center'
        }}>
          <Link 
            to="/books"
            style={{
              color: '#6b7280',
              textDecoration: 'none',
              fontSize: '14px',
              fontWeight: '500'
            }}
          >
            Books
          </Link>


          {user && (
            <>
              <Link 
                to="/profile"
                style={{
                  color: '#6b7280',
                  textDecoration: 'none',
                  fontSize: '14px',
                  fontWeight: '500'
                }}
              >
                Profile
              </Link>
              <Link 
                to="/orders"
                style={{
                  color: '#6b7280',
                  textDecoration: 'none',
                  fontSize: '14px',
                  fontWeight: '500'
                }}
              >
                Orders
              </Link>
            </>
          )}


          {/* Cart */}
          <Link 
            to="/cart"
            style={{
              display: 'flex',
              alignItems: 'center',
              gap: '8px',
              color: '#208092',
              textDecoration: 'none',
              fontWeight: '600'
            }}
          >
            <ShoppingCart style={{ width: '20px', height: '20px' }} />
            Cart
          </Link>


          {/* User Section */}
          <div style={{ borderLeft: '1px solid #e5e7eb', paddingLeft: '20px' }}>
            {user ? (
              <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                <Link 
                  to="/profile"
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: '6px',
                    fontSize: '14px',
                    color: '#1f2937',
                    textDecoration: 'none',
                    padding: '6px 12px',
                    borderRadius: '6px',
                    transition: 'background 0.2s'
                  }}
                  onMouseEnter={(e) => e.target.style.background = '#f3f4f6'}
                  onMouseLeave={(e) => e.target.style.background = 'transparent'}
                >
                  <User style={{ width: '16px', height: '16px' }} />
                  {user.username}
                </Link>
                <button
                  onClick={handleLogout}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: '6px',
                    background: '#fee2e2',
                    color: '#dc2626',
                    border: 'none',
                    padding: '6px 12px',
                    borderRadius: '6px',
                    cursor: 'pointer',
                    fontSize: '13px',
                    fontWeight: '600'
                  }}
                >
                  <LogOut style={{ width: '16px', height: '16px' }} />
                  Logout
                </button>
              </div>
            ) : (
              <div style={{ display: 'flex', gap: '12px' }}>
                <Link 
                  to="/login"
                  style={{
                    background: '#208092',
                    color: 'white',
                    padding: '8px 16px',
                    borderRadius: '6px',
                    textDecoration: 'none',
                    fontSize: '13px',
                    fontWeight: '600'
                  }}
                >
                  Login
                </Link>
                <Link 
                  to="/register"
                  style={{
                    background: 'transparent',
                    color: '#208092',
                    padding: '8px 16px',
                    borderRadius: '6px',
                    border: '1px solid #208092',
                    textDecoration: 'none',
                    fontSize: '13px',
                    fontWeight: '600'
                  }}
                >
                  Register
                </Link>
              </div>
            )}
          </div>
        </nav>
      </div>
    </header>
  );
}
