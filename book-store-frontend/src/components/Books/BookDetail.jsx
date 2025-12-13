import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Star, ShoppingCart } from 'lucide-react';
import { booksAPI } from '../../api/books';
import { useCartStore } from '../../store/cartStore';
import { useAuthStore } from '../../store/authStore';
import { ReviewList } from '../Reviews/ReviewList';
import { ReviewForm } from '../Reviews/ReviewForm';

export function BookDetail() {
  const { id } = useParams();
  const [book, setBook] = useState(null);
  const [loading, setLoading] = useState(true);
  const { addToCart } = useCartStore();
  const { user } = useAuthStore();

  useEffect(() => {
    fetchBook();
  }, [id]);

  const fetchBook = async () => {
    try {
      const response = await booksAPI.getBookById(id);
      setBook(response.data);
    } catch (error) {
      console.error('Failed to fetch book:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async () => {
    if (!user) {
      alert('Please login first');
      return;
    }
    try {
      await addToCart(id);
      alert('Added to cart!');
    } catch (error) {
      alert('Failed to add to cart');
    }
  };

  if (loading) return <div className="text-center py-12">Loading...</div>;
  if (!book) return <div className="text-center py-12">Book not found</div>;

  const avgRating = Math.round(book.averageRating * 10) / 10;

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      <div className="grid md:grid-cols-3 gap-8 mb-12">
        <div className="md:col-span-1">
          <div className="bg-gradient-to-br from-primary to-accent h-96 rounded-lg flex items-center justify-center">
            <span className="text-white text-8xl">📖</span>
          </div>
        </div>

        <div className="md:col-span-2">
          <h1 className="text-4xl font-bold mb-4">{book.title}</h1>
          <p className="text-xl text-gray-600 mb-2">By {book.authorName}</p>
          <p className="text-gray-500 mb-4">{book.genre}</p>

          <div className="flex items-center gap-4 mb-6">
            <div className="flex items-center">
              <Star className="w-6 h-6 fill-yellow-400 text-yellow-400" />
              <span className="text-2xl ml-2">{avgRating}</span>
              <span className="text-gray-600 ml-2">({book.reviewCount} reviews)</span>
            </div>
          </div>

          <p className="text-gray-700 mb-6 leading-relaxed">{book.description}</p>

          <div className="flex items-center gap-6 mb-8">
            <span className="text-3xl font-bold text-primary">${book.price}</span>
            <button
              onClick={handleAddToCart}
              className="flex items-center gap-2 bg-primary text-white px-6 py-3 rounded-lg hover:opacity-90 transition"
            >
              <ShoppingCart className="w-6 h-6" />
              Add to Cart
            </button>
          </div>
        </div>
      </div>

      <div className="border-t pt-8">
        <h2 className="text-2xl font-bold mb-6">Reviews</h2>
        {user && (
          <ReviewForm bookId={id} onSuccess={fetchBook} />
        )}
        <ReviewList bookId={id} />
      </div>
    </div>
  );
}
