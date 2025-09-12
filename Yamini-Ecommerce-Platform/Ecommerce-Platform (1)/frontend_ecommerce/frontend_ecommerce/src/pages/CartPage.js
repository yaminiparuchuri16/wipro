import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { removeFromCart } from '../redux/slices/cartSlice';
import { Link } from 'react-router-dom';

export default function CartPage(){
  const cart = useSelector(s => Array.isArray(s.cart.items) ? s.cart.items : []);
  const dispatch = useDispatch();
  const total = cart.reduce((s,i)=>s + (i.unitPrice || i.price || 0) * (i.quantity || 1), 0);

  return (
    <div style={{display:'flex',gap:20}}>
      <div style={{flex:2}}>
        <h3>Your Cart</h3>
        {cart.length === 0 ? <p>Cart is empty</p> : (
          <div>
            {cart.map((i,idx)=> (
              <div key={idx} className="card" style={{display:'flex',justifyContent:'space-between',alignItems:'center',marginBottom:8}}>
                <div>
                  <div style={{fontWeight:600}}>{i.name || `Product #${i.productId}`}</div>
                  <div className="small">Qty: {i.quantity}</div>
                </div>
                <div style={{textAlign:'right'}}>
                  <div>₹{i.unitPrice || i.price || 0}</div>
                  <button className="button flipkart-btn" style={{background:'#e53935',color:'#fff',border:'none',borderRadius:4,padding:'8px 16px',fontWeight:'bold',boxShadow:'0 1px 4px #e0e0e0'}} onClick={()=>dispatch({type:'cart/removeFromCartFrontend',payload:{productId:i.productId}})}>Remove</button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
      <aside style={{width:320}} className="card aside-cart">
        <h4>Order Summary</h4>
        <p className="small">Items: {cart.length}</p>
        <p className="price">Total: ₹{total}</p>
        <Link to="/checkout"><button className="button" disabled={cart.length===0}>Proceed to checkout</button></Link>
      </aside>
    </div>
  );
}
