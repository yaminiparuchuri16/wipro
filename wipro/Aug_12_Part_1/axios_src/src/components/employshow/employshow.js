import axios from 'axios';
import React, {Component, useEffect, useState} from 'react';

const EmployShow = () => {

  const [employs,setEmployData] = useState([])

  useEffect(() => {
    const fetchData = async () => {
      const response = await
        axios.get("https://localhost:7209/api/Employs");
        setEmployData(response.data)
    }
    fetchData();
  },[])  
  return(
    <div>
      <table border="3" align="center">
        <tr>
          <th>Employ No</th>
          <th>Employ Name</th>
          <th>Gender</th>
          <th>Department</th>
          <th>Designation</th>
          <th>Basic</th>
        </tr>
        {employs.map((item) =>
        <tr>
          <td>{item.empno}</td>
           <td>{item.name}</td>
            <td>{item.gender}</td>
           <td>{item.dept}</td>
           <td>{item.desig}</td>
           <td>{item.basic}</td>
        </tr>
      )}
      </table>
    </div>
  )
}

export default EmployShow;