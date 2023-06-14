import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;
  
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      userName: ''
    };
  }

  componentDidMount() {
    this.checkAuthentication();
    this.getUserName();
  }

  checkAuthentication() {
    const token = localStorage.getItem('token');
    const isAuthenticated = !!token;
    
    this.setState({ isAuthenticated });
  }

  getUserName() {
  
    const token = localStorage.getItem('token');
    
    if (token) {
      fetch('/api/users/current', {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })
        .then(response => response.json())
        .then(data => {
          const userName = data.username;
          this.setState({ userName });
        })
        .catch(error => {
          console.error('Failed to get user name:', error);
        });
    }
  }

  render() {
    const { isAuthenticated, userName } = this.state;

    return (
      <div>
        <h1>My Book List</h1>
        {isAuthenticated ? (
          <div>
            <p>Welcome, {userName}!</p>
            {<p>Glad to see you again!</p>}
          </div>
        ) : (
          <div>
            <p>Welcome to your book storage:</p>
            <p>Register to make your own list</p>
          </div>
        )}
      </div>
    );
  }
}
