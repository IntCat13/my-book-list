import React, { Component } from 'react';

export class RegisterForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      login: '',
      password: '',
      error: ''
    };
  }

  handleInputChange = (event) => {
    this.setState({ [event.target.name]: event.target.value });
  }

  handleSubmit = async (e) => {
      e.preventDefault();
  
      const { login, password } = this.state;
  
      try {
        const response = await fetch('/api/auth/register', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ login, password })
        });
  
        if (response.ok) {
          window.location.assign('/');
        }
      } catch (error) {
        // Handle any network or server errors
        console.error('Login error:', error);
      }
    };

  render() {

    return (
      <div>
        <h2>Register</h2>
        <form onSubmit={this.handleSubmit}>
          <div>
            <label>Login:</label>
            <input type="text" name="login" onChange={this.handleInputChange} />
          </div>
          <div>
            <label>Password:</label>
            <input type="password" name="password" onChange={this.handleInputChange} />
          </div>
          <button type="submit">Register</button>
        </form>
      </div>
    );
  }
}
