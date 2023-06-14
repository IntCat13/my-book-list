import React, { useEffect, useState } from 'react';
import BookForm from './BookForm';

const UserBookList = () => {
    const [books, setBooks] = useState([]);
    const [userBooks, setUserBooks] = useState([]);
    const [selectedBookId, setSelectedBookId] = useState(null);
    const token = localStorage.getItem('token');

    useEffect(() => {
        const requestOptions = {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        };

        fetch('/api/books', requestOptions)
            .then((response) => response.json())
            .then((data) => setBooks(data));
    }, [token]);

    useEffect(() => {
        const requestOptions = {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        };

        fetch('/api/users/books', requestOptions)
            .then((response) => response.json())
            .then((data) => setUserBooks(data));
    }, [token]);

    const handleRemoveFromList = (bookId) => {
        const requestOptions = {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`,
                    },
                    body: JSON.stringify({ bookId }),
                };
        
                fetch(`/api/users/handleBook`, requestOptions)
                    .then((response) => response.json())
                    .then((data) => {
                        setUserBooks(data);
                    })
                    .catch((error) => {
                        console.error(error);
                    });
    };

    const handleEditBook = (bookId) => {
        setSelectedBookId(bookId);
    };

    const handleSaveBook = (updatedBookData) => {
            const requestOptions = {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(updatedBookData),
            };
    
            fetch(`/api/users/books`, requestOptions)
                .then((response) => response.json())
                .then((data) => {
                    setUserBooks((prevUserBooks) =>
                        prevUserBooks.map((userBook) => {
                            if (userBook.bookId === updatedBookData.bookId) {
                                return { ...userBook, ...data };
                            }
                            return userBook;
                        })
                    );
                    setSelectedBookId(null);
                    window.location.assign('/MyList');
                })
                .catch((error) => {
                    console.error(error);
                });
        };


    const getBookData = (bookId) => {
        const book = books.find((book) => book.id === bookId);
        return book ? { title: book.title, author: book.author, genre: book.genre } : {};
    };

    return (
        <main>
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Author</th>
                        <th>Genre</th>
                        <th>Status</th>
                        <th>Read</th>
                        <th>Rereads</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {userBooks.map((userBook) => {
                        const { bookId, status, pagesRead, reReadCount } = userBook;
                        const { title, author, genre } = getBookData(bookId);
                        return (
                            <React.Fragment key={bookId}>
                                <tr>
                                    <td>{title}</td>
                                    <td>{author}</td>
                                    <td>{genre}</td>
                                    <td>{status}</td>
                                    <td>{pagesRead}</td>
                                    <td>{reReadCount}</td>
                                    <td>
                                        <button onClick={() => handleEditBook(bookId)}>Edit</button>
                                        <button onClick={() => handleRemoveFromList(bookId)}>Remove</button>
                                    </td>
                                </tr>
                                {selectedBookId === bookId && (
                                    <tr>
                                        <td colSpan="4">
                                            <BookForm
                                                bookId={bookId}
                                                status={status}
                                                pagesRead={pagesRead}
                                                reReadCount={reReadCount}
                                                onSave={handleSaveBook}
                                            />
                                        </td>
                                    </tr>
                                )}
                            </React.Fragment>
                        );
                    })}
                </tbody>
            </table>
        </main>
    );
};

export default UserBookList;
