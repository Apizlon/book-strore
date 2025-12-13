import { Link } from 'react-router-dom';
import { Star, ShoppingCart, Plus } from 'lucide-react'; // ✅ ShoppingCartPlus → Plus
import { useState } from 'react';

export function BookCard({ book, user }) {
  const [addingToCart, setAddingToCart] = useState(false);
  const [cartError, setCartError] = useState('');

  const handleAddToCart = async (e) => {
    e.preventDefault();
    e.stopPropagation();
    
    if (!user) {
      alert('Please login first');
      return;
    }

    setAddingToCart(true);
    setCartError('');

    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/cart/add', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ bookId: book.id })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to add to cart');
      }

      const data = await response.json();
      console.log('✅ Added to cart:', data);
      alert(`✅ "${book.title}" added to cart!`);
    } catch (err) {
      console.error('❌ Cart error:', err);
      setCartError(err.message);
    } finally {
      setAddingToCart(false);
    }
  };

  const avgRating = book.averageRating ? Math.round(book.averageRating * 10) / 10 : 0;

  return (
    <Link to={`/books/${book.id}`} className="block">
      <div style={{
        background: '#fff',
        borderRadius: '12px',
        boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
        overflow: 'hidden',
        transition: 'all 0.3s ease'
      }}>
        <div style={{
          height: '200px',
          background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}>
          <span style={{ fontSize: '48px' }}>📖</span>
        </div>
        
        <div style={{ padding: '20px' }}>
          <h3 style={{ fontSize: '18px', fontWeight: 'bold', marginBottom: '8px' }}>
            {book.title}
          </h3>
          <p style={{ color: '#6b7280', fontSize: '14px', marginBottom: '4px' }}>
            {book.authorName}
          </p>
          <p style={{ color: '#9ca3af', fontSize: '12px', marginBottom: '12px' }}>
            {book.genre}
          </p>
          
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '16px' }}>
            <Star style={{ width: '16px', height: '16px', color: '#fbbf24' }} />
            <span style={{ fontSize: '14px', fontWeight: '600' }}>
              {avgRating} ({book.reviewCount || 0})
            </span>
          </div>

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', paddingTop: '16px', borderTop: '1px solid #e5e7eb' }}>
            <span style={{ fontSize: '20px', fontWeight: 'bold', color: '#208092' }}>
              ${book.price}
            </span>
            
            <button
              onClick={handleAddToCart}
              disabled={addingToCart || !user}
              style={{
                display: 'flex',
                alignItems: 'center',
                gap: '6px',
                padding: '10px 16px',
                background: user ? '#208092' : '#9ca3af',
                color: 'white',
                border: 'none',
                borderRadius: '8px',
                fontWeight: '600',
                cursor: user && !addingToCart ? 'pointer' : 'not-allowed',
                opacity: addingToCart || !user ? 0.6 : 1
              }}
            >
              {addingToCart ? (
                'Adding...'
              ) : (
                <>
                  <ShoppingCart style={{ width: '16px', height: '16px' }} />
                  <Plus style={{ width: '16px', height: '16px' }} />
                  Add
                </>
              )}
            </button>
          </div>

          {cartError && (
            <div style={{
              marginTop: '8px',
              padding: '8px',
              background: '#fef2f2',
              color: '#dc2626',
              borderRadius: '6px',
              fontSize: '12px'
            }}>
              {cartError}
            </div>
          )}
        </div>
      </div>
    </Link>
  );
}
