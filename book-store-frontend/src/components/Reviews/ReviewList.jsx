import { useEffect, useState } from 'react';
import { booksAPI } from '../../api/books';
import { Star, Trash2 } from 'lucide-react';
import { useAuthStore } from '../../store/authStore';

export function ReviewList({ bookId }) {
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const { user } = useAuthStore();

  useEffect(() => {
    fetchReviews();
  }, [bookId]);

  const fetchReviews = async () => {
    try {
      const response = await booksAPI.getReviewsByBook(bookId);
      setReviews(response.data);
    } catch (error) {
      console.error('Failed to fetch reviews:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteReview = async (reviewId) => {
    if (!confirm('Are you sure?')) return;
    try {
      await booksAPI.deleteReview(reviewId);
      setReviews(reviews.filter(r => r.id !== reviewId));
    } catch (error) {
      alert('Failed to delete review');
    }
  };

  if (loading) return <div>Loading reviews...</div>;

  return (
    <div className="space-y-4">
      {reviews.length === 0 ? (
        <p className="text-gray-500">No reviews yet. Be the first to review!</p>
      ) : (
        reviews.map(review => (
          <div key={review.id} className="bg-white p-4 rounded-lg border">
            <div className="flex justify-between items-start mb-2">
              <div>
                <p className="font-semibold">{review.username}</p>
                <div className="flex items-center gap-2">
                  <div className="flex">
                    {[...Array(5)].map((_, i) => (
                      <Star
                        key={i}
                        className={`w-4 h-4 ${i < review.rating ? 'fill-yellow-400 text-yellow-400' : 'text-gray-300'}`}
                      />
                    ))}
                  </div>
                  <span className="text-sm text-gray-500">{new Date(review.createdAt).toLocaleDateString()}</span>
                </div>
              </div>
              {user && (user.id === review.username || user.role === 'Admin') && (
                <button
                  onClick={() => handleDeleteReview(review.id)}
                  className="text-red-500 hover:text-red-700"
                >
                  <Trash2 className="w-5 h-5" />
                </button>
              )}
            </div>
            <p className="text-gray-700">{review.text}</p>
          </div>
        ))
      )}
    </div>
  );
}
