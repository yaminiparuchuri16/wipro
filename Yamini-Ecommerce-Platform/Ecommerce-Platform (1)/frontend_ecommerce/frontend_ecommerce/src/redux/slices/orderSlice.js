import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

export const placeOrder = createAsyncThunk('orders/place', async (payload) => {
  // payload: { items: [ { productId, quantity } ] }
  const res = await api.post('/Orders/create', payload);
  return res.data;
});

export const fetchOrders = createAsyncThunk('orders/fetch', async () => {
  const res = await api.get('/Orders');
  return res.data;
});

const slice = createSlice({
  name: 'orders',
  initialState: { items: [], status: 'idle' },
  reducers: {},
  extraReducers: (builder)=>{
    builder.addCase(placeOrder.fulfilled, (state, action)=>{ state.status='placed'; });
    builder.addCase(fetchOrders.fulfilled, (state, action)=>{ state.items = action.payload; });
  }
});

export default slice.reducer;
