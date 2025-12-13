import axios from 'axios';
import { API_BASE_URL } from '../utils/constants';

const api = axios.create({
  baseURL: `${API_BASE_URL}/api/auth`,
});

export const authAPI = {
  register: (data) => api.post('/register', data),
  login: (data) => api.post('/login', data),
  refresh: (token) => api.post('/refresh', { refreshToken: token }),
  me: (token) => {
    return api.get('/me', {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
};
