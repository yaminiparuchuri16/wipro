
import React, { useEffect, useState } from 'react';
import api from '../services/api';

export default function Admin(){
  const [products, setProducts] = useState([]);
  const [name,setName]=useState(''); const [price,setPrice]=useState(''); const [desc,setDesc]=useState('');
  useEffect(()=>{ api.get('/Products').then(r=>setProducts(r.data)).catch(()=>{}); },[]);

  const add = async ()=>{
    try{
      await api.post('/Products', { name, description: desc, price: parseFloat(price), imageUrl: '' });
      // Fetch updated products
      const res = await api.get('/Products');
      setProducts(res.data);
      // Clear input fields
      setName('');
      setPrice('');
      setDesc('');
      alert('Product added!');
    }catch(e){ alert('Failed'); }
  };

  return (
    <div>
      <h3>Admin</h3>
      <div style={{display:'flex',gap:20}}>
        <div style={{flex:2}}>
          <h4>Products</h4>
          {products.map(p=> <div key={p.id} className="card" style={{marginBottom:8}}>{p.name} — ₹{p.price}</div>)}
        </div>
        <aside style={{width:320}} className="card aside-cart">
          <h4>Add Product</h4>
          <input className="input" placeholder="Name" value={name} onChange={e=>setName(e.target.value)} />
          <input className="input" placeholder="Price" value={price} onChange={e=>setPrice(e.target.value)} />
          <input className="input" placeholder="Description" value={desc} onChange={e=>setDesc(e.target.value)} />
          <button className="button" onClick={add}>Add</button>
        </aside>
      </div>
    </div>
  );
}
