import { useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Trash2, ShoppingCart } from 'lucide-react';
import { useCartStore } from '../../store/cartStore';

export function CartPage() {
  const { cart, isLoading, fetchCart, removeFromCart, checkout } = useCartStore();
  const navigate = useNavigate();

  useEffect(() => {
    fetchCart();
  }, []);

  const handleCheckout = async () => {
    try {
      const order = await checkout();
      alert('Order placed successfully!');
      navigate(`/orders/${order.id}`);
    } catch (error) {
      alert(error.response?.data?.error || 'Checkout failed');
    }
  };

  if (isLoading) return <div className="text-center py-12">Loading...</div>;

  const items = cart?.items || [];
  const total = items.reduce((sum, item) => sum + item.bookPrice, 0);

  return (
    <div className="max-w-7xl mx-auto px-4 py-8">
      <h1 className="text-4xl font-bold mb-8">Shopping Cart</h1>

      {items.length === 0 ? (
        <div className="text-center py-12">
          <ShoppingCart className="w-16 h-16 mx-auto text-gray-300 mb-4" />
          <p className="text-xl text-gray-600 mb-4">Your cart is empty</p>
          <Link to="/books" className="text-primary hover:underline">
            Continue Shopping
          </Link>
        </div>
      ) : (
        <div className="grid md:grid-cols-3 gap-8">
          <div className="md:col-span-2 space-y-4">
            {items.map(item => (
              <div key={item.id} className="bg-white p-4 rounded-lg border flex justify-between items-center">
                <div className="flex-1">
                  <h3 className="font-semibold text-lg">{item.bookTitle}</h3>
                  <p className="text-gray-600">${item.bookPrice.toFixed(2)}</p>
                  <p className="text-sm text-gray-500">Added {new Date(item.addedAt).toLocaleDateString()}</p>
                </div>
                <button
                  onClick={() => removeFromCart(item.id)}
                  className="text-red-500 hover:text-red-700 p-2"
                >
                  <Trash2 className="w-5 h-5" />
                </button>
              </div>
            ))}
          </div>

          <div className="bg-gray-50 p-6 rounded-lg h-fit">
            <h2 className="font-bold text-xl mb-4">Order Summary</h2>
            <div className="space-y-3 mb-6 pb-6 border-b">
              <div className="flex justify-between">
                <span>Subtotal ({items.length} items)</span>
                <span>${total.toFixed(2)}</span>
              </div>
              <div className="flex justify-between">
                <span>Shipping</span>
                <span>FREE</span>
              </div>
              <div className="flex justify-between">
                <span>Tax</span>
                <span>${(total * 0.1).toFixed(2)}</span>
              </div>
            </div>
            <div className="flex justify-between font-bold text-lg mb-4">
              <span>Total</span>
              <span>${(total * 1.1).toFixed(2)}</span>
            </div>
            <button
              onClick={handleCheckout}
              className="w-full bg-primary text-white py-3 rounded-lg hover:opacity-90 transition"
            >
              Checkout
            </button>
            <Link
              to="/books"
              className="block text-center mt-3 text-primary hover:underline"
            >
              Continue Shopping
            </Link>
          </div>
        </div>
      )}
    </div>
  );
}
