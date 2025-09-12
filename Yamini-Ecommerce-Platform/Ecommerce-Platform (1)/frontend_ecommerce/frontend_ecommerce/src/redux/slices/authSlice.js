import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

const token = localStorage.getItem('token') || null;
const user = JSON.parse(localStorage.getItem('user') || 'null');

export const login = createAsyncThunk('auth/login', async (payload, thunkAPI) => {
  // payload: { email, password }
  const res = await api.post('/Auth/login', payload);
  return res.data;
});

export const register = createAsyncThunk('auth/register', async (payload) => {
  // payload: { email, password }
  const res = await api.post('/Auth/register', payload);
  return res.data;
});

const authSlice = createSlice({
  name: 'auth',
  initialState: { token, user, status: 'idle', error: null },
  reducers: {
    logout(state){ state.token = null; state.user = null; localStorage.removeItem('token'); localStorage.removeItem('user'); }
  },
  extraReducers: (builder)=>{
    builder.addCase(login.fulfilled, (state, action)=>{
      state.token = action.payload.token;
      state.user = { email: action.payload.email, role: action.payload.role };
      localStorage.setItem('token', action.payload.token);
      localStorage.setItem('user', JSON.stringify(state.user));
      state.status = 'succeeded';
    });
    builder.addCase(login.rejected, (state, action)=>{ state.error = 'Login failed'; state.status='failed'; });
    builder.addCase(register.fulfilled, (state, action)=>{ state.status='registered'; });
  }
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;
