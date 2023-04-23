import React, { useState, useEffect } from 'react';
import styles from './BattleshipGrid.module.css';

const BattleshipGrid = ({ game }) => {
    const [grid, setGrid] = useState(
        Array.from({ length: 10 }, () => new Array(10).fill(0))
    );

    useEffect(() => {
        if (game && game.ships) {
            const newGrid = Array.from({ length: 10 }, () => new Array(10).fill(0));

            game.ships.forEach((ship) => {
                const shipLength = ship.type;
                const row = ship.row - 1;
                const column = ship.column.charCodeAt(0) - 'A'.charCodeAt(0);

                if (ship.orientation === 0) {
                    for (let i = 0; i < shipLength; i++) {
                        newGrid[row][column + i] = 1;
                    }
                } else {
                    for (let i = 0; i < shipLength; i++) {
                        newGrid[row + i][column] = 1;
                    }
                }
            });

            setGrid(newGrid);
        }
    }, [game]);

    return (
        <table className={styles.table}>
            <thead>
            <tr>
                <th></th>
                {['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'].map((col, index) => (
                    <th key={index} className={styles.cell}>{col}</th>
                ))}
            </tr>
            </thead>
            <tbody>
            {grid.map((row, rowIndex) => (
                <tr key={rowIndex}>
                    <td className={styles.cell}>{rowIndex + 1}</td>
                    {row.map((cell, columnIndex) => (
                        <td key={columnIndex} className={`${styles.cell} ${cell === 1 ? styles.ship : ''}`}>{cell === 1 ? 'X' : ''}</td>
                    ))}
                </tr>
            ))}
            </tbody>
        </table>
    );
};

export default BattleshipGrid;
