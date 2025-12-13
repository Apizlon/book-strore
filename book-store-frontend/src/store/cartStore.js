import { create } from 'zustand';
import { booksAPI } from '../api/books';

export const useCartStore = create((set) => ({
  cart: null,
  isLoading: false,
  error: null,

  fetchCart: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await booksAPI.getCart();
      set({ cart: response.data, isLoading: false });
    } catch (error) {
      set({ error: error.response?.data?.error || 'Failed to fetch cart', isLoading: false });
    }
  },

  addToCart: async (bookId) => {
    set({ isLoading: true, error: null });
    try {
      const response = await booksAPI.addToCart({ bookId });
      set({ cart: response.data, isLoading: false });
    } catch (error) {
      set({ error: error.response?.data?.error || 'Failed to add to cart', isLoading: false });
      throw error;
    }
  },

  removeFromCart: async (itemId) => {
    set({ isLoading: true, error: null });
    try {
      const response = await booksAPI.removeFromCart(itemId);
      set({ cart: response.data, isLoading: false });
    } catch (error) {
      set({ error: error.response?.data?.error || 'Failed to remove from cart', isLoading: false });
    }
  },

  clearCart: async () => {
    set({ isLoading: true, error: null });
    try {
      await booksAPI.clearCart();
      set({ cart: null, isLoading: false });
    } catch (error) {
      set({ error: error.response?.data?.error || 'Failed to clear cart', isLoading: false });
    }
  },

  checkout: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await booksAPI.checkout();
      set({ cart: null, isLoading: false });
      return response.data;
    } catch (error) {
      set({ error: error.response?.data?.error || 'Checkout failed', isLoading: false });
      throw error;
    }
  },
}));
