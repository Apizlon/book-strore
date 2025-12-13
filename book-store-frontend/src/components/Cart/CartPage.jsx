import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Trash2, ShoppingCart, CreditCard, ChevronLeft } from 'lucide-react';

export function CartPage() {
  const navigate = useNavigate();
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);
  const [checkingOut, setCheckingOut] = useState(false);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch (e) {
        console.error('Failed to parse user:', e);
      }
    }
    fetchCart();
  }, []);

  const fetchCart = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/cart', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to fetch cart');
      }

      const data = await response.json();
      console.log('✅ Cart loaded:', data);
      setCart(data);
    } catch (err) {
      console.error('❌ Cart fetch error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleRemove = async (itemId) => {
    try {
      const token = localStorage.getItem('token');
      const response = await fetch(`http://localhost:5003/api/cart/item/${itemId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to remove item');
      }

      console.log('✅ Item removed from cart');
      fetchCart();
    } catch (err) {
      console.error('❌ Remove error:', err);
      alert('Failed to remove item from cart');
    }
  };

  const handleCheckout = async () => {
    setCheckingOut(true);

    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/order/checkout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Checkout failed');
      }

      const order = await response.json();
      console.log('✅ Order created:', order);
      alert('✅ Order placed successfully!');
      navigate('/orders');
    } catch (err) {
      console.error('❌ Checkout error:', err);
      alert('❌ Checkout failed: ' + err.message);
    } finally {
      setCheckingOut(false);
    }
  };

  if (!user) {
    return (
      <div style={{ padding: '100px 20px', textAlign: 'center', maxWidth: '1200px', margin: '0 auto' }}>
        <h2 style={{ fontSize: '28px', color: '#6b7280', marginBottom: '20px' }}>
          Please login to view your cart
        </h2>
        <Link
          to="/login"
          style={{
            display: 'inline-block',
            background: '#208092',
            color: 'white',
            padding: '12px 24px',
            borderRadius: '8px',
            textDecoration: 'none',
            fontWeight: '600'
          }}
        >
          Go to Login
        </Link>
      </div>
    );
  }

  if (loading) {
    return (
      <div style={{ padding: '100px', textAlign: 'center', fontSize: '24px' }}>
        Loading cart...
      </div>
    );
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div style={{ padding: '100px 20px', textAlign: 'center', maxWidth: '1200px', margin: '0 auto' }}>
        <ShoppingCart style={{ width: '80px', height: '80px', color: '#d1d5db', margin: '0 auto 20px' }} />
        <h2 style={{ fontSize: '28px', color: '#6b7280', marginBottom: '20px' }}>
          Your cart is empty
        </h2>
        <Link
          to="/books"
          style={{
            display: 'inline-block',
            background: '#208092',
            color: 'white',
            padding: '12px 24px',
            borderRadius: '8px',
            textDecoration: 'none',
            fontWeight: '600'
          }}
        >
          Continue Shopping
        </Link>
      </div>
    );
  }

  return (
    <div style={{ padding: '40px 20px', maxWidth: '1200px', margin: '0 auto' }}>
      <Link 
        to="/books" 
        style={{ 
          display: 'flex', 
          alignItems: 'center', 
          gap: '8px', 
          color: '#6b7280', 
          textDecoration: 'none',
          padding: '12px',
          marginBottom: '40px'
        }}
      >
        <ChevronLeft style={{ width: '18px', height: '18px' }} />
        Back to Books
      </Link>

      <h1 style={{ fontSize: '28px', fontWeight: 'bold', marginBottom: '32px', color: '#1f2937' }}>
        🛒 Shopping Cart ({cart.items.length} {cart.items.length === 1 ? 'item' : 'items'})
      </h1>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 350px', gap: '32px' }}>
        {/* Cart Items */}
        <div>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
            {cart.items.map((item) => (
              <div
                key={item.id}
                style={{
                  display: 'grid',
                  gridTemplateColumns: '100px 1fr 80px',
                  gap: '16px',
                  alignItems: 'center',
                  padding: '12px',
                  background: '#fff',
                  borderRadius: '8px',
                  border: '1px solid #e5e7eb'
                }}
              >
                <div style={{
                  width: '100px',
                  height: '130px',
                  background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                  borderRadius: '6px',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  fontSize: '40px'
                }}>
                  📖
                </div>

                <div>
                  <h3 style={{ fontSize: '14px', fontWeight: '600', marginBottom: '4px', color: '#1f2937' }}>
                    {item.bookTitle}
                  </h3>
                  <p style={{ fontSize: '13px', color: '#9ca3af' }}>
                    ${item.bookPrice}
                  </p>
                </div>

                <button
                  onClick={() => handleRemove(item.id)}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    width: '36px',
                    height: '36px',
                    background: '#fee2e2',
                    color: '#dc2626',
                    border: 'none',
                    borderRadius: '6px',
                    cursor: 'pointer'
                  }}
                >
                  <Trash2 style={{ width: '16px', height: '16px' }} />
                </button>
              </div>
            ))}
          </div>
        </div>

        {/* Order Summary */}
        <div style={{
          padding: '16px',
          background: '#f8fafc',
          borderRadius: '8px',
          border: '1px solid #e5e7eb',
          height: 'fit-content',
          position: 'sticky',
          top: '20px'
        }}>
          <h2 style={{ fontSize: '16px', fontWeight: '600', marginBottom: '16px', color: '#1f2937' }}>
            Order Summary
          </h2>

          <div style={{ marginBottom: '12px', fontSize: '13px' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '6px' }}>
              <span style={{ color: '#6b7280' }}>Subtotal:</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>${cart.totalPrice.toFixed(2)}</span>
            </div>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '6px' }}>
              <span style={{ color: '#6b7280' }}>Shipping:</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>FREE</span>
            </div>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '6px' }}>
              <span style={{ color: '#6b7280' }}>Tax:</span>
              <span style={{ fontWeight: '600', color: '#1f2937' }}>$0.00</span>
            </div>
          </div>

          <div style={{
            paddingTop: '12px',
            borderTop: '1px solid #e5e7eb',
            display: 'flex',
            justifyContent: 'space-between',
            marginBottom: '16px'
          }}>
            <span style={{ fontWeight: '600', color: '#1f2937' }}>Total:</span>
            <span style={{ fontSize: '16px', fontWeight: '700', color: '#208092' }}>
              ${cart.totalPrice.toFixed(2)}
            </span>
          </div>

          <button
            onClick={handleCheckout}
            disabled={checkingOut || !cart.items.length}
            style={{
              width: '100%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '8px',
              padding: '12px',
              background: checkingOut ? '#9ca3af' : '#208092',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              fontSize: '13px',
              fontWeight: '600',
              cursor: checkingOut || !cart.items.length ? 'not-allowed' : 'pointer'
            }}
          >
            {checkingOut ? (
              <>
                <div style={{ 
                  width: '14px', 
                  height: '14px', 
                  border: '2px solid #ffffff40', 
                  borderTop: '2px solid white', 
                  borderRadius: '50%', 
                  animation: 'spin 1s linear infinite' 
                }} />
                Processing...
              </>
            ) : (
              <>
                <CreditCard style={{ width: '16px', height: '16px' }} />
                Checkout
              </>
            )}
          </button>
        </div>
      </div>

      <style>{`
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
      `}</style>
    </div>
  );
}
