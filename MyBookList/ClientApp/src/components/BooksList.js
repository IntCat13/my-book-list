import React, { useEffect, useState } from 'react';

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [userBooks, setUserBooks] = useState([]);
    const token = localStorage.getItem('token');

    useEffect(() => {
        fetch('/api/books')
            .then((response) => response.json())
            .then((data) => setBooks(data));
    }, []);

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

    const handleAddToList = (bookId) => {
        const requestOptions = {
            method: isBookInList(bookId) ? 'DELETE' : 'POST',
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

    const isBookInList = (bookId) => {
        return userBooks.some((userBook) => userBook.bookId === bookId);
    };

    const getBookStatus = (bookId) => {
        const userBook = userBooks.find((userBook) => userBook.bookId === bookId);
        return userBook ? userBook.status : 'Not Read';
    };

    return (
        <main>
            {books.length > 0 ? (
                <table className="table table-striped" aria-labelledby="tableLabel">
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Author</th>
                            <th>Genre</th>
                            <th>Status</th>
                            {token && <th>Action</th>}
                        </tr>
                    </thead>
                    <tbody>
                        {books.map((book) => (
                            <tr key={book.id}>
                                <td>{book.title}</td>
                                <td>{book.author}</td>
                                <td>{book.genre}</td>
                                <td>{getBookStatus(book.id)}</td>
                                {token && (
                                    <td>
                                        <button onClick={() => handleAddToList(book.id)}>
                                            {isBookInList(book.id) ? 'Remove from List' : 'Add to List'}
                                        </button>
                                    </td>
                                )}
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p>Loading...</p>
            )}
        </main>
    );
};

export default BookList;
