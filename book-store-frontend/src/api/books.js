import axios from 'axios';
import { BOOK_SERVICE_URL } from '../utils/constants';
import { tokenStorage } from '../utils/tokenStorage';

const api = axios.create({
  baseURL: `${BOOK_SERVICE_URL}/api`,
});

api.interceptors.request.use((config) => {
  const token = tokenStorage.getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const booksAPI = {
  // Books
  getAllBooks: () => api.get('/books'),
  getBookById: (id) => api.get(`/books/${id}`),
  getBooksByAuthor: (authorId) => api.get(`/books/by-author/${authorId}`),
  getBooksByGenre: (genre) => api.get(`/books/by-genre/${genre}`),
  getTopRatedBooks: (take = 10) => api.get(`/books/top-rated?take=${take}`),
  createBook: (data) => api.post('/books', data),
  updateBook: (id, data) => api.put(`/books/${id}`, data),
  deleteBook: (id) => api.delete(`/books/${id}`),

  // Authors
  getAllAuthors: () => api.get('/authors'),
  getAuthorById: (id) => api.get(`/authors/${id}`),
  createAuthor: (data) => api.post('/authors', data),
  updateAuthor: (id, data) => api.put(`/authors/${id}`, data),
  deleteAuthor: (id) => api.delete(`/authors/${id}`),

  // Reviews
  getReviewsByBook: (bookId) => api.get(`/reviews/book/${bookId}`),
  createReview: (data) => api.post('/reviews', data),
  deleteReview: (id) => api.delete(`/reviews/${id}`),

  // Cart
  getCart: () => api.get('/cart'),
  addToCart: (data) => api.post('/cart/add', data),
  removeFromCart: (itemId) => api.delete(`/cart/item/${itemId}`),
  clearCart: () => api.post('/cart/clear'),

  // Orders
  checkout: () => api.post('/order/checkout'),
  getOrder: (id) => api.get(`/order/${id}`),
  getUserOrders: () => api.get('/order'),
};
