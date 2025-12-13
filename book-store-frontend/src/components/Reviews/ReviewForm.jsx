import { useState } from 'react';
import { booksAPI } from '../../api/books';

export function ReviewForm({ bookId, onSuccess }) {
  const [rating, setRating] = useState(5);
  const [text, setText] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await booksAPI.createReview({
        bookId,
        rating: parseInt(rating),
        text
      });
      setText('');
      setRating(5);
      onSuccess?.();
    } catch (err) {
      setError(err.response?.data?.error || 'Failed to submit review');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-gray-50 p-6 rounded-lg mb-8">
      <h3 className="font-bold text-lg mb-4">Leave a Review</h3>
      
      {error && <div className="bg-red-100 text-red-700 p-3 rounded mb-4">{error}</div>}

      <div className="mb-4">
        <label className="block text-sm font-medium mb-2">Rating</label>
        <select
          value={rating}
          onChange={(e) => setRating(e.target.value)}
          className="w-full px-3 py-2 border rounded-lg"
        >
          <option value="1">⭐ 1 - Poor</option>
          <option value="2">⭐⭐ 2 - Fair</option>
          <option value="3">⭐⭐⭐ 3 - Good</option>
          <option value="4">⭐⭐⭐⭐ 4 - Very Good</option>
          <option value="5">⭐⭐⭐⭐⭐ 5 - Excellent</option>
        </select>
      </div>

      <div className="mb-4">
        <label className="block text-sm font-medium mb-2">Your Review</label>
        <textarea
          value={text}
          onChange={(e) => setText(e.target.value)}
          required
          minLength="10"
          maxLength="500"
          className="w-full px-3 py-2 border rounded-lg resize-none"
          rows="4"
          placeholder="Share your thoughts about this book..."
        />
      </div>

      <button
        type="submit"
        disabled={loading}
        className="bg-primary text-white px-6 py-2 rounded-lg hover:opacity-90 transition disabled:opacity-50"
      >
        {loading ? 'Submitting...' : 'Submit Review'}
      </button>
    </form>
  );
}
