import React, { useEffect, useState } from 'react';

type User = {
    id: number;
    name: string;
    surname: string;
    email: string;
    courseName: string;
    completionDate: string;
    certificateStatus: string;
};

export default function UsersGrid() {
    const [users, setUsers] = useState<User[]>([]);
    const [total, setTotal] = useState(0);
    const [page, setPage] = useState(1);
    const pageSize = 20;

    useEffect(() => {
        const fetchUsers = async () => {
            const res = await fetch(`https://localhost:7216/grid?page=${page}&pageSize=${pageSize}`);
            const data = await res.json();

            setUsers(data.data);
            setTotal(data.total);
        };

        fetchUsers();
    }, [page]);

    const totalPages = Math.ceil(total / pageSize);

    return (
        <div>
            <table className="border w-full text-left">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Surname</th>
                        <th>Email</th>
                        <th>Course</th>
                        <th>Completed</th>
                        <th>Certificate</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(u => (
                        <tr key={u.id}>
                            <td>{u.id}</td>
                            <td>{u.name}</td>
                            <td>{u.surname}</td>
                            <td>{u.email}</td>
                            <td>{u.courseName}</td>
                            <td>{u.completionDate}</td>
                            <td>{u.certificateStatus}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <div className="flex gap-2 mt-4">
                <button onClick={() => setPage(p => Math.max(p - 1, 1))} disabled={page === 1}>Prev</button>
                <span>Page {page} of {totalPages}</span>
                <button onClick={() => setPage(p => Math.min(p + 1, totalPages))} disabled={page === totalPages}>Next</button>
            </div>
        </div>
    );
}