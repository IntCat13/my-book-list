import React, { Component } from 'react';

export class LoginForm extends Component {
  constructor(props) {
    super(props);
    this.state = {
      login: '',
      password: ''
    };
  }

  handleChange = (e) => {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  };

  handleSubmit = async (e) => {
    e.preventDefault();

    const { login, password } = this.state;

    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ login, password })
      });

      if (response.ok) {
        const { token } = await response.json();
        // Store the token in local storage or session storage
        localStorage.setItem('token', token);
        window.location.assign('/');
      }
    } catch (error) {
      // Handle any network or server errors
      console.error('Login error:', error);
    }
  };

  render() {
    const { login, password } = this.state;

    return (
      <form onSubmit={this.handleSubmit}>
        <input
          type="text"
          name="login"
          placeholder="Login"
          value={login}
          onChange={this.handleChange}
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={password}
          onChange={this.handleChange}
        />
        <button type="submit">Login</button>
      </form>
    );
  }
}
