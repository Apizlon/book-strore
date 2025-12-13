export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5001';
export const BOOK_SERVICE_URL = import.meta.env.VITE_BOOK_SERVICE_URL || 'http://localhost:5003';

export const BOOK_GENRES = [
  'Fiction',
  'Mystery',
  'ScienceFiction',
  'Fantasy',
  'Romance',
  'Thriller',
  'Biography',
  'History',
  'Technology',
  'SelfHelp',
  'Poetry',
  'Drama',
  'Adventure',
  'Horror',
  'Educational'
];

export const USER_ROLES = {
  USER: 'User',
  MODERATOR: 'Moderator',
  ADMIN: 'Admin'
};

export const PERMISSIONS = {
  READ_OWN_PROFILE: 'ReadOwnProfile',
  UPDATE_OWN_PROFILE: 'UpdateOwnProfile',
  READ_BOOKS: 'ReadBooks',
  ADD_REVIEW: 'AddReview',
  ADD_BOOK: 'AddBook',
  EDIT_BOOK: 'EditBook',
  DELETE_BOOK: 'DeleteBook',
  ADD_AUTHOR: 'AddAuthor',
  EDIT_AUTHOR: 'EditAuthor',
  DELETE_AUTHOR: 'DeleteAuthor',
};
