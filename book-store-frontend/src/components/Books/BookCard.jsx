import { Link } from 'react-router-dom';
import { Star, ShoppingCart } from 'lucide-react';
import { useCartStore } from '../../store/cartStore';
import { useAuthStore } from '../../store/authStore';

export function BookCard({ book }) {
  const { addToCart } = useCartStore();
  const { user } = useAuthStore();
  const avgRating = Math.round(book.averageRating * 10) / 10;

  const handleAddToCart = async (e) => {
    e.preventDefault();
    if (!user) {
      alert('Please login first');
      return;
    }
    try {
      await addToCart(book.id);
      alert('Added to cart!');
    } catch (error) {
      alert('Failed to add to cart');
    }
  };

  return (
    <Link to={`/books/${book.id}`}>
      <div className="bg-white rounded-lg shadow hover:shadow-lg transition overflow-hidden">
        <div className="bg-gradient-to-br from-primary to-accent h-48 flex items-center justify-center">
          <span className="text-white text-4xl">📖</span>
        </div>
        <div className="p-4">
          <h3 className="font-bold text-lg truncate">{book.title}</h3>
          <p className="text-gray-600 text-sm">{book.authorName}</p>
          <p className="text-gray-500 text-xs mt-2">{book.genre}</p>
          
          <div className="flex items-center gap-2 mt-3">
            <div className="flex items-center">
              <Star className="w-4 h-4 fill-yellow-400 text-yellow-400" />
              <span className="text-sm ml-1">{avgRating}</span>
              <span className="text-xs text-gray-500">({book.reviewCount})</span>
            </div>
          </div>

          <div className="flex justify-between items-center mt-4 pt-4 border-t">
            <span className="font-bold text-lg text-primary">${book.price}</span>
            <button
              onClick={handleAddToCart}
              className="bg-primary text-white p-2 rounded hover:opacity-90 transition"
            >
              <ShoppingCart className="w-5 h-5" />
            </button>
          </div>
        </div>
      </div>
    </Link>
  );
}
