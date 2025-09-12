import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

export const fetchProducts = createAsyncThunk('products/fetch', async () => {
  const res = await api.get('/Products');
  return res.data;
});

const slice = createSlice({
  name: 'products',
  initialState: { items: [], status: 'idle' },
  reducers: {},
  extraReducers: (builder)=>{
    builder.addCase(fetchProducts.fulfilled, (state, action)=>{ state.items = action.payload; state.status='succeeded'; });
    builder.addCase(fetchProducts.pending, (state)=>{ state.status='loading'; });
  }
});

export default slice.reducer;
