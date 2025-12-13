import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Star, ShoppingCart, Plus, ChevronLeft } from 'lucide-react';
import { ReviewForm } from '../Reviews/ReviewForm';

export function BookDetail() {
  const { id } = useParams();
  const [book, setBook] = useState(null);
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);
  const [addingToCart, setAddingToCart] = useState(false);

  useEffect(() => {
    fetchBook();
    fetchReviews();
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser));
      } catch (e) {
        console.error('Failed to parse user:', e);
      }
    }
  }, [id]);

  const fetchBook = async () => {
    try {
      const response = await fetch(`http://localhost:5003/api/books/${id}`);
      if (!response.ok) throw new Error('Book not found');
      const data = await response.json();
      console.log('✅ Book loaded:', data);
      setBook(data);
    } catch (err) {
      console.error('❌ Book error:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchReviews = async () => {
    try {
      const response = await fetch(`http://localhost:5003/api/reviews/book/${id}`);
      if (!response.ok) throw new Error('Failed to fetch reviews');
      const data = await response.json();
      console.log('✅ Reviews loaded:', data);
      setReviews(data);
    } catch (err) {
      console.error('❌ Reviews fetch error:', err);
    }
  };

  const handleAddToCart = async (e) => {
    e.preventDefault();
    if (!user) {
      alert('Please login first');
      return;
    }

    setAddingToCart(true);

    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/cart/add', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ bookId: id })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to add to cart');
      }

      console.log('✅ Added to cart');
      alert(`✅ "${book.title}" added to cart!`);
    } catch (err) {
      console.error('❌ Cart error:', err);
      alert('❌ Failed to add to cart: ' + err.message);
    } finally {
      setAddingToCart(false);
    }
  };

  if (loading) {
    return (
      <div style={{ padding: '100px', textAlign: 'center', fontSize: '24px' }}>
        Loading book details...
      </div>
    );
  }

  if (!book) {
    return (
      <div style={{ padding: '100px', textAlign: 'center' }}>
        <h2 style={{ color: '#6b7280', fontSize: '28px' }}>Book not found</h2>
        <Link 
          to="/books" 
          style={{ 
            display: 'inline-block', 
            marginTop: '20px', 
            background: '#3b82f6', 
            color: 'white', 
            padding: '12px 24px', 
            borderRadius: '8px', 
            textDecoration: 'none' 
          }}
        >
          ← Back to Books
        </Link>
      </div>
    );
  }

  const avgRating = book.averageRating ? Math.round(book.averageRating * 10) / 10 : 0;

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

      <div style={{ display: 'grid', gridTemplateColumns: '300px 1fr', gap: '40px', marginBottom: '60px' }}>
        <div>
          <div style={{
            width: '100%',
            height: '320px',
            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
            borderRadius: '12px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginBottom: '20px',
            fontSize: '80px'
          }}>
            📖
          </div>
          
          <span style={{ 
            fontSize: '20px', 
            fontWeight: 'bold', 
            color: '#208092',
            display: 'block',
            marginBottom: '16px'
          }}>
            ${book.price}
          </span>
          
          <button
            onClick={handleAddToCart}
            disabled={addingToCart || !user}
            style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '8px',
              width: '100%',
              padding: '12px',
              background: user ? '#208092' : '#9ca3af',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              fontSize: '14px',
              fontWeight: '600',
              cursor: user && !addingToCart ? 'pointer' : 'not-allowed'
            }}
          >
            {addingToCart ? 'Adding...' : (
              <>
                <ShoppingCart style={{ width: '16px', height: '16px' }} />
                Add to Cart
              </>
            )}
          </button>
          
          {!user && (
            <div style={{
              padding: '12px',
              background: '#fef3c7',
              borderRadius: '8px',
              color: '#92400e',
              fontSize: '12px',
              marginTop: '12px',
              textAlign: 'center'
            }}>
              <Link to="/login" style={{ color: '#b45309', fontWeight: '600' }}>Login</Link> to buy
            </div>
          )}
        </div>

        <div>
          <h1 style={{ 
            fontSize: '32px', 
            fontWeight: 'bold', 
            marginBottom: '12px', 
            color: '#1f2937'
          }}>
            {book.title}
          </h1>
          
          <p style={{ 
            fontSize: '16px', 
            color: '#374151', 
            marginBottom: '8px'
          }}>
            {book.authorName}
          </p>
          
          <p style={{ 
            color: '#6b7280', 
            fontSize: '13px', 
            marginBottom: '20px',
            textTransform: 'uppercase',
            fontWeight: '600'
          }}>
            {book.genre}
          </p>

          <div style={{ 
            display: 'flex', 
            alignItems: 'center', 
            gap: '12px', 
            marginBottom: '20px',
            paddingBottom: '20px',
            borderBottom: '1px solid #e5e7eb'
          }}>
            <div style={{ 
              display: 'flex', 
              gap: '2px' 
            }}>
              {[...Array(5)].map((_, i) => (
                <Star 
                  key={i} 
                  style={{ 
                    width: '16px', 
                    height: '16px', 
                    color: i < avgRating ? '#fbbf24' : '#d1d5db',
                    fill: i < avgRating ? '#fbbf24' : 'none'
                  }} 
                />
              ))}
            </div>
            <span style={{ fontSize: '14px', fontWeight: '600' }}>
              {avgRating} ({reviews.length} {reviews.length === 1 ? 'review' : 'reviews'})
            </span>
          </div>

          <p style={{ 
            lineHeight: '1.6', 
            color: '#374151', 
            fontSize: '14px' 
          }}>
            {book.description || 'No description available.'}
          </p>
        </div>
      </div>

      <div style={{ borderTop: '1px solid #e5e7eb', paddingTop: '40px' }}>
        <h2 style={{ 
          fontSize: '20px', 
          marginBottom: '24px', 
          color: '#1f2937',
          fontWeight: '600'
        }}>
          Reviews
        </h2>
        
        {user && (
          <div style={{ marginBottom: '32px' }}>
            <ReviewForm 
              bookId={id} 
              user={user} 
              onSuccess={() => {
                fetchBook();
                fetchReviews();
              }} 
            />
          </div>
        )}

        {reviews.length === 0 ? (
          <p style={{ color: '#6b7280', fontSize: '14px' }}>
            No reviews yet. {user ? 'Be the first to write one!' : <Link to="/login" style={{ color: '#208092' }}>Login</Link>}
          </p>
        ) : (
          <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
            {reviews.map((review) => (
              <div key={review.id} style={{ padding: '12px', border: '1px solid #e5e7eb', borderRadius: '6px' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '6px' }}>
                  <div>
                    <span style={{ fontWeight: '600', fontSize: '13px', color: '#1f2937' }}>
                      {review.username}
                    </span>
                    <p style={{ color: '#9ca3af', fontSize: '12px', margin: '2px 0 0 0' }}>
                      {new Date(review.createdAt).toLocaleDateString()}
                    </p>
                  </div>
                  <div style={{ display: 'flex', gap: '2px' }}>
                    {[...Array(5)].map((_, i) => (
                      <Star 
                        key={i} 
                        style={{ 
                          width: '12px', 
                          height: '12px', 
                          color: i < review.rating ? '#fbbf24' : '#d1d5db',
                          fill: i < review.rating ? '#fbbf24' : 'none'
                        }} 
                      />
                    ))}
                  </div>
                </div>
                <p style={{ color: '#374151', fontSize: '13px', lineHeight: '1.4', marginTop: '6px' }}>
                  {review.text}
                </p>
              </div>
            ))}
          </div>
        )}
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
