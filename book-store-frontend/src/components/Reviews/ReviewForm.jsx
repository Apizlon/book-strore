import { useState } from 'react';

export function ReviewForm({ bookId, user, onSuccess }) {
  const [rating, setRating] = useState(5);
  const [text, setText] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSubmitting(true);

    try {
      const token = localStorage.getItem('token');
      const response = await fetch('http://localhost:5003/api/reviews', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
          bookId,
          rating: parseInt(rating),
          text
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to submit review');
      }

      console.log('✅ Review created');
      setText('');
      setRating(5);
      onSuccess?.();
      alert('✅ Review submitted successfully!');
    } catch (err) {
      console.error('❌ Review error:', err);
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  };

  if (!user) return null;

  return (
    <form onSubmit={handleSubmit} style={{
      background: '#f8fafc',
      padding: '24px',
      borderRadius: '12px',
      marginBottom: '32px',
      border: '1px solid #e2e8f0'
    }}>
      <h3 style={{ fontSize: '20px', fontWeight: 'bold', marginBottom: '20px', color: '#1e293b' }}>
        ✍️ Write a Review
      </h3>
      
      {error && (
        <div style={{
          background: '#fef2f2',
          color: '#dc2626',
          padding: '12px',
          borderRadius: '8px',
          marginBottom: '16px',
          borderLeft: '4px solid #dc2626',
          fontSize: '14px'
        }}>
          {error}
        </div>
      )}

      <div style={{ marginBottom: '16px' }}>
        <label style={{ display: 'block', marginBottom: '8px', fontWeight: '600' }}>Rating</label>
        <select
          value={rating}
          onChange={(e) => setRating(e.target.value)}
          disabled={submitting}
          style={{
            width: '100%',
            padding: '12px',
            border: '2px solid #e2e8f0',
            borderRadius: '8px',
            fontSize: '16px',
            background: submitting ? '#f1f5f9' : 'white'
          }}
        >
          <option value="1">⭐ 1 - Terrible</option>
          <option value="2">⭐⭐ 2 - Poor</option>
          <option value="3">⭐⭐⭐ 3 - Average</option>
          <option value="4">⭐⭐⭐⭐ 4 - Good</option>
          <option value="5">⭐⭐⭐⭐⭐ 5 - Excellent</option>
        </select>
      </div>

      <div style={{ marginBottom: '20px' }}>
        <label style={{ display: 'block', marginBottom: '8px', fontWeight: '600' }}>Your Review</label>
        <textarea
          value={text}
          onChange={(e) => setText(e.target.value)}
          disabled={submitting}
          minLength="10"
          maxLength="1000"
          rows="4"
          placeholder="Share your honest thoughts about this book..."
          style={{
            width: '100%',
            padding: '12px',
            border: '2px solid #e2e8f0',
            borderRadius: '8px',
            fontSize: '14px',
            fontFamily: 'inherit',
            resize: 'vertical',
            background: submitting ? '#f1f5f9' : 'white'
          }}
        />
        <div style={{ fontSize: '12px', color: '#64748b', marginTop: '4px' }}>
          {text.length}/1000 characters
        </div>
      </div>

      <button 
        type="submit" 
        disabled={submitting || text.length < 10}
        style={{
          width: '100%',
          padding: '14px',
          background: (submitting || text.length < 10) ? '#94a3b8' : '#208092',
          color: 'white',
          border: 'none',
          borderRadius: '8px',
          fontSize: '16px',
          fontWeight: '600',
          cursor: (submitting || text.length < 10) ? 'not-allowed' : 'pointer'
        }}
      >
        {submitting ? '⏳ Submitting...' : '🚀 Submit Review'}
      </button>
    </form>
  );
}
