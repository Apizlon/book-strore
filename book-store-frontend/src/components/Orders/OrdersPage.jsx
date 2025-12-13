import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

export function OrdersPage() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchOrders();
  }, []);

  const fetchOrders = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/order', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to fetch orders');
      }

      const data = await response.json();
      console.log('✅ Orders:', data);
      setOrders(data);
    } catch (err) {
      console.error('❌ Orders error:', err);
      setError('Failed to load orders');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div style={{ padding: '50px', textAlign: 'center' }}>Loading orders...</div>;
  if (error) return <div style={{ padding: '50px', textAlign: 'center', color: 'red' }}>{error}</div>;

  return (
    <div style={{ padding: '50px', maxWidth: '1200px', margin: '0 auto' }}>
      <h1 style={{ fontSize: '32px', marginBottom: '30px' }}>My Orders</h1>
      
      {orders.length === 0 ? (
        <div style={{ textAlign: 'center', padding: '50px', color: '#666' }}>
          <h3>No orders yet</h3>
          <p>Start shopping to see your orders here!</p>
          <Link 
            to="/books" 
            style={{ 
              background: '#208092', 
              color: 'white', 
              padding: '12px 24px', 
              borderRadius: '8px', 
              textDecoration: 'none',
              display: 'inline-block',
              marginTop: '20px'
            }}
          >
            Go Shopping
          </Link>
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
          {orders.map(order => (
            <div key={order.id} style={{ 
              border: '1px solid #eee', 
              borderRadius: '8px', 
              padding: '24px',
              background: '#f9f9f9'
            }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
                <h3 style={{ fontSize: '24px', margin: 0 }}>Order #{order.id.slice(-6)}</h3>
                <span style={{ 
                  fontSize: '18px', 
                  fontWeight: 'bold', 
                  color: order.status === 'Completed' ? '#10b981' : '#f59e0b'
                }}>
                  {order.status}
                </span>
              </div>
              <div style={{ display: 'flex', gap: '20px', flexWrap: 'wrap' }}>
                <div style={{ flex: 1, minWidth: '200px' }}>
                  <p><strong>Date:</strong> {new Date(order.createdAt).toLocaleDateString()}</p>
                  <p><strong>Total:</strong> ${order.totalPrice?.toFixed(2) || '0.00'}</p>
                  <p><strong>Items:</strong> {order.items?.length || 0}</p>
                </div>
                <div style={{ flex: 1, minWidth: '200px' }}>
                  <p><strong>Shipping:</strong> {order.shippingAddress || 'TBD'}</p>
                </div>
              </div>
              {order.items && order.items.length > 0 && (
                <div style={{ marginTop: '20px' }}>
                  <h4 style={{ marginBottom: '10px' }}>Items:</h4>
                  <div style={{ display: 'flex', gap: '10px', flexWrap: 'wrap' }}>
                    {order.items.map((item, index) => (
                      <div key={index} style={{ 
                        padding: '12px', 
                        border: '1px solid #ddd', 
                        borderRadius: '4px',
                        minWidth: '150px'
                      }}>
                        <div>{item.bookTitle}</div>
                        <div>${item.price?.toFixed(2)}</div>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
