import axios from 'axios';
import { API_BASE_URL } from '../utils/constants';
import { tokenStorage } from '../utils/tokenStorage';

const api = axios.create({
  baseURL: `${API_BASE_URL}/api/users`,
});

api.interceptors.request.use((config) => {
  const token = tokenStorage.getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const usersAPI = {
  getProfile: () => api.get('/me'),
  updateProfile: (data) => api.put('/me', data),
  getUserById: (id) => api.get(`/${id}`),
};
