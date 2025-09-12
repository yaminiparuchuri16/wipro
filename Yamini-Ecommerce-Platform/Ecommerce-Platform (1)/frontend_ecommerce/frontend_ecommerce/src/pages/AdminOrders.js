import React, { useEffect, useState } from 'react';

const ORDER_STATUSES = ['Pending', 'Shipped', 'Delivered', 'Cancelled'];

function getOrders() {
  return JSON.parse(localStorage.getItem('orders') || '[]');
}
function setOrders(orders) {
  localStorage.setItem('orders', JSON.stringify(orders));
}

export default function AdminOrders() {
  const [orders, setOrdersState] = useState([]);

  useEffect(() => {
    setOrdersState(getOrders());
  }, []);

  const deleteOrder = (id) => {
    if (!window.confirm('Delete this order?')) return;
    const updated = orders.filter(o => o.id !== id);
    setOrders(updated);
    setOrdersState(updated);
  };

  const changeStatus = (id, status) => {
    const updated = orders.map(o => o.id === id ? { ...o, status } : o);
    setOrders(updated);
    setOrdersState(updated);
  };

  return (
    <div>
      <h2>Admin Order Management</h2>
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
  );
}
