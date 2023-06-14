import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      isAuthenticated: false
    };
  }

  componentDidMount() {
    const token = localStorage.getItem('token');
    const isAuthenticated = !!token;

    this.setState({ isAuthenticated });
  }
  
  handleLogOut() {
    localStorage.removeItem('token');
    window.location.assign('/');
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    const { isAuthenticated } = this.state;

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">MyBookList</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              {isAuthenticated ? (
                <>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/books">Books</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/mylist">My books</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink onClick={() => this.handleLogOut()}>Logout</NavLink>
                  </NavItem>
                </>
              ) : (
                <>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/books">Books</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/register">Register</NavLink>
                  </NavItem>
                </>
              )}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
