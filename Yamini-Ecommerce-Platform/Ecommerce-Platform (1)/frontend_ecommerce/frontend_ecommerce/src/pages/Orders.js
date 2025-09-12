
export default function Orders(){
  const user = JSON.parse(localStorage.getItem('user') || 'null');
  const allOrders = JSON.parse(localStorage.getItem('orders') || '[]');
  const orders = user ? allOrders.filter(o => o.user === user.email) : [];

  return (
    <div>
      <h3>Your Orders</h3>
      {orders.length === 0 ? <p>No orders yet</p> : orders.map(o => (
        <div key={o.id} className="card" style={{marginBottom:8}}>
          <div><strong>Order #{o.id}</strong></div>
          <div>Status: {o.status}</div>
          <div>Total: ₹{o.total}</div>
          <div>Items: {o.items.map(i => i.name + ' x' + i.quantity).join(', ')}</div>
        </div>
      ))}
    </div>
  );
}
