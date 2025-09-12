import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../services/api';

export const fetchCart = createAsyncThunk('cart/fetch', async () => {
  const res = await api.get('/Cart');
  return res.data;
});

export const addToCart = createAsyncThunk('cart/add', async ({ productId, quantity }) => {
  try {
    const res = await api.post('/Cart/add', { productId, quantity });
    if (res.data && Array.isArray(res.data)) {
      return res.data;
    }
  } catch (e) {}
  // Fallback: update cart in frontend state
  const state = getState();
  const items = state.cart.items.slice();
  const idx = items.findIndex(i => i.productId === productId);
  if (idx !== -1) {
    items[idx].quantity += quantity;
  } else {
    items.push({ productId, quantity });
  }
  return items;
});

export const removeFromCart = createAsyncThunk('cart/remove', async ({ productId }) => {
  try {
    const res = await api.post('/Cart/remove', { productId });
    if (res.data && Array.isArray(res.data)) {
      return res.data;
    }
  } catch (e) {}
  // Fallback: update cart in frontend state
  const state = getState();
  const items = state.cart.items.filter(i => i.productId !== productId);
  return items;
});

const slice = createSlice({
  name: 'cart',
  initialState: { items: [], status: 'idle' },
  reducers: {
    clearCart(state){ state.items = []; },
    addToCartFrontend(state, action) {
      const { productId, quantity, name, price, imageUrl } = action.payload;
      const idx = state.items.findIndex(i => i.productId === productId);
      if (idx !== -1) {
        state.items[idx].quantity += quantity;
      } else {
        state.items.push({ productId, quantity, name, price, imageUrl });
      }
      state.status = 'succeeded';
    },
    removeFromCartFrontend(state, action) {
      const { productId } = action.payload;
      state.items = state.items.filter(i => i.productId !== productId);
      state.status = 'succeeded';
    }
  },
  extraReducers: (builder) => {
    builder.addCase(fetchCart.fulfilled, (state, action) => {
      state.items = action.payload;
      state.status = 'succeeded';
    });
    builder.addCase(addToCart.fulfilled, (state, action) => {
      state.items = action.payload;
      state.status = 'succeeded';
    });
    builder.addCase(removeFromCart.fulfilled, (state, action) => {
      state.items = action.payload;
      state.status = 'succeeded';
    });
    builder.addCase(fetchCart.pending, (state) => { state.status = 'loading'; });
    builder.addCase(addToCart.pending, (state) => { state.status = 'loading'; });
    builder.addCase(removeFromCart.pending, (state) => { state.status = 'loading'; });
  }
});

export const { clearCart } = slice.actions;
export default slice.reducer;
