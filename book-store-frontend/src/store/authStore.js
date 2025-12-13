import { create } from 'zustand';
import { tokenStorage } from '../utils/tokenStorage';
import { authAPI } from '../api/auth';

export const useAuthStore = create((set) => ({
  user: tokenStorage.getUser(),
  token: tokenStorage.getToken(),
  isLoading: false,
  error: null,

  login: async (email, password) => {
    set({ isLoading: true, error: null });
    try {
      const response = await authAPI.login({ email, password });
      const { token, user } = response.data;
      
      tokenStorage.setToken(token);
      tokenStorage.setUser(user);
      
      set({ user, token, isLoading: false });
      return user;
    } catch (error) {
      set({ error: error.response?.data?.error || 'Login failed', isLoading: false });
      throw error;
    }
  },

  register: async (username, email, password) => {
    set({ isLoading: true, error: null });
    try {
      const response = await authAPI.register({ username, email, password });
      const { token, user } = response.data;
      
      tokenStorage.setToken(token);
      tokenStorage.setUser(user);
      
      set({ user, token, isLoading: false });
      return user;
    } catch (error) {
      set({ error: error.response?.data?.error || 'Registration failed', isLoading: false });
      throw error;
    }
  },

  logout: () => {
    tokenStorage.clear();
    set({ user: null, token: null, error: null });
  },

  setError: (error) => set({ error }),
  clearError: () => set({ error: null }),
}));
