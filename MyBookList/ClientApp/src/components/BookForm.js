import React, { useState } from 'react';

const BookForm = ({ bookId, status, pagesRead, reReadCount, onSave }) => {
    const [updatedStatus, setUpdatedStatus] = useState(status);
    const [updatedPagesRead, setUpdatedPagesRead] = useState(pagesRead);
    const [updatedReReadCount, setUpdatedReReadCount] = useState(reReadCount);

    const handleSubmit = (e) => {
        e.preventDefault();
        const updatedBookData = {
            bookId,
            status: updatedStatus,
            pagesRead: updatedPagesRead,
            reReadCount: updatedReReadCount,
        };
        onSave(updatedBookData);

        setUpdatedStatus('');
        setUpdatedPagesRead(0);
        setUpdatedReReadCount(0);
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label>Status:</label>
                <input type="text" value={updatedStatus} onChange={(e) => setUpdatedStatus(e.target.value)} />
            </div>
            <div>
                <label>Pages Read:</label>
                <input
                    type="number"
                    value={updatedPagesRead}
                    onChange={(e) => setUpdatedPagesRead(parseInt(e.target.value))}
                />
            </div>
            <div>
                <label>Re-Read Count:</label>
                <input
                    type="number"
                    value={updatedReReadCount}
                    onChange={(e) => setUpdatedReReadCount(parseInt(e.target.value))}
                />
            </div>
            <button type="submit">Save Changes</button>
        </form>
    );
};

export default BookForm;
