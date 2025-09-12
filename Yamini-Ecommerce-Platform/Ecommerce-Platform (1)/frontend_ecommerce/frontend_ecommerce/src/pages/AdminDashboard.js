import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import api from '../services/api';

const ORDER_STATUSES = ['Pending', 'Shipped', 'Delivered', 'Cancelled'];

function getOrders() {
  return JSON.parse(localStorage.getItem('orders') || '[]');
}
function setOrders(orders) {
  localStorage.setItem('orders', JSON.stringify(orders));
}

export default function AdminDashboard() {
  const nav = useNavigate();
  const user = useSelector(s => s.auth.user);
  const [products, setProducts] = useState([]);
  const [orders, setOrdersState] = useState([]);
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [desc, setDesc] = useState('');
  const [editingId, setEditingId] = useState(null);

  useEffect(() => {
    api.get('/Products').then(r => setProducts(r.data)).catch(() => {});
    setOrdersState(getOrders());
  }, []);

  // Only allow access if admin is logged in
  if (localStorage.getItem('adminLoggedIn') !== 'true') {
    nav('/admin-login');
    return null;
  }

  // Product management
  const addProduct = async () => {
    if (!name.trim()) {
      alert('Product name is required');
      return;
    }
    const priceValue = parseFloat(price);
    if (isNaN(priceValue) || priceValue <= 0) {
      alert('Price must be a valid number greater than 0');
      return;
    }
    try {
      if (editingId) {
        await api.put(`/Products/${editingId}`, { name, description: desc, price: priceValue, imageUrl: '' });
        alert('Product updated!');
      } else {
        await api.post('/Products', { name, description: desc, price: priceValue, imageUrl: '' });
        alert('Product added!');
      }
      const res = await api.get('/Products');
      setProducts(res.data);
      setName(''); setPrice(''); setDesc(''); setEditingId(null);
    } catch (e) { alert('Failed to add/update product'); }
  };

  const deleteProduct = async (id) => {
    if (!window.confirm('Delete this product?')) return;
    try {
      await api.delete(`/Products/${id}`);
      setProducts(products.filter(p => p.id !== id));
    } catch (e) { alert('Delete failed'); }
  };

  // Order management
  const changeStatus = (id, status) => {
    const updated = orders.map(o => o.id === id ? { ...o, status } : o);
    setOrders(updated);
    setOrdersState(updated);
    localStorage.setItem('orders', JSON.stringify(updated));
  };

  const deleteOrder = (id) => {
    if (!window.confirm('Delete this order?')) return;
    const updated = orders.filter(o => o.id !== id);
    setOrders(updated);
    setOrdersState(updated);
    localStorage.setItem('orders', JSON.stringify(updated));
  };

  const handleLogout = () => {
    localStorage.removeItem('adminLoggedIn');
    nav('/admin-login');
  };

  return (
    <div>
      <div style={{display:'flex',justifyContent:'space-between',alignItems:'center'}}>
        <h2>Admin Dashboard</h2>
        <button className="button secondary" onClick={handleLogout}>Logout</button>
      </div>
      <div style={{display:'flex',gap:40,alignItems:'flex-start'}}>
        <div style={{flex:2}}>
          <h3>Products</h3>
          <div style={{marginBottom:24}}>
            <input className="input" placeholder="Name" value={name} onChange={e=>setName(e.target.value)} />
            <input className="input" placeholder="Price" value={price} onChange={e=>setPrice(e.target.value)} />
            <input className="input" placeholder="Description" value={desc} onChange={e=>setDesc(e.target.value)} />
            <button className="button" onClick={addProduct}>{editingId ? 'Update Product' : 'Add Product'}</button>
            {editingId && <button className="button secondary" style={{marginLeft:8}} onClick={()=>{setName('');setPrice('');setDesc('');setEditingId(null);}}>Cancel</button>}
          </div>
          {products.map(p => (
              <div key={p.id} className="card" style={{marginBottom:8,display:'flex',justifyContent:'space-between',alignItems:'center'}}>
                <span>{p.name} — ₹{p.price}</span>
                <div style={{display:'flex',gap:8}}>
                  <button className="button secondary" onClick={() => {
                    setName(p.name);
                    setPrice(p.price.toString());
                    setDesc(p.description || '');
                    setEditingId(p.id);
                    window.scrollTo({top:0,behavior:'smooth'});
                  }}>Edit</button>
                  <button className="button secondary" onClick={() => deleteProduct(p.id)}>Delete</button>
                  <button className="button secondary" onClick={() => nav(`/product/${p.id}`)} style={{ marginLeft: "8px" }}>View Details</button>
                </div>
              </div>
          ))}
        </div>
        <div style={{flex:2}}>
          <h3>Orders</h3>
          {orders.length === 0 ? (
            <div>No orders found.</div>
          ) : (
            <table style={{width:'100%',background:'#fff',borderRadius:8,boxShadow:'0 2px 8px #eee',marginTop:24}}>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>User</th>
                  <th>Items</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {orders.map(order => (
                  <tr key={order.id}>
                    <td>{order.id}</td>
                    <td>{order.user || 'Guest'}</td>
                    <td>{order.items.map(i => i.name + ' x' + i.quantity).join(', ')}</td>
                    <td>
                      <select value={order.status} onChange={e => changeStatus(order.id, e.target.value)}>
                        {ORDER_STATUSES.map(s => <option key={s} value={s}>{s}</option>)}
                      </select>
                    </td>
                    <td>
                      <button className="button secondary" onClick={() => deleteOrder(order.id)}>Delete</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
}
