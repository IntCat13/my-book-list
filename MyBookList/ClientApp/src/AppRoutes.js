// AppRoutes.js
import { Home } from "./components/Home";
import { LoginForm } from "./components/LoginForm";
import { RegisterForm } from "./components/RegisterForm";
import BooksList from "./components/BooksList";
import UserBookList from "./components/UserBookList";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/login',
    element: <LoginForm />
  },
  {
    path: '/register',
    element: <RegisterForm />
  },
  {
    path: '/books',
    element: <BooksList />
  },
  {
     path: '/MyList',
     element: <UserBookList />
  }
];

export default AppRoutes;
