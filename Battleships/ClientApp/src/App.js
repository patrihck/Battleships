import React, {useEffect, useState} from 'react';
import axios from 'axios';
import styles from './BattleshipGame.module.css';
import BattleshipGrid from "./BattleshipGrid";

const BattleshipGame = () => {
    const [selectedRow, setSelectedRow] = useState('');
    const [selectedColumn, setSelectedColumn] = useState('');
    const [turns, setTurns] = useState(0);
    const [shipsSunkenCount, setShipsSunkenCount] = useState(0);
    const [result, setResult] = useState('');
    const [error, setError] = useState('');
    const [game, setGame] = useState(null);
    const [gameStatus, setGameStatus] = useState('playing');

    const createGame = async () => {
        try {
            const response = await axios.get('api/game');
            setGame(response.data);
            setGameStatus('playing');
        } catch (error) {
            setError('An error occurred while creating the game.');
        }
    };

    const makeMove = async () => {
        try {
            const response = await axios.put(`api/game/${game.id}/move`, {
                row: parseInt(selectedRow),
                column: selectedColumn,
            });

            const { data } = response;
            if (data.errorOccured) {
                setError(data.errorMessage);
            } else {
                setTurns(data.turns);
                setShipsSunkenCount(data.shipsSunkenCount);
                setResult(data.result);
            }
        } catch (error) {
            setError('An error occurred while making the move.');
        }
    };

    useEffect(() => {
        if (shipsSunkenCount === 3) {
            setGameStatus('gameOver');
        }
    }, [shipsSunkenCount]);


    return (
        <div className={styles.container}>
            {game === null || gameStatus === 'gameOver' ? (
                <div
                    style={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "center",
                    }}
                >
                    <h1 className={styles.title}>Battleship Game</h1>
                    <button className={styles.button} onClick={createGame}>
                        Create game
                    </button>
                </div>
            ) : (
                <div>
                    <div className={styles.inputs}>
                        <select
                            className={styles.select}
                            value={selectedRow}
                            onChange={(e) => setSelectedRow(e.target.value)}
                        >
                            <option value="">Row</option>
                            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((row) => (
                                <option key={row} value={row}>
                                    {row}
                                </option>
                            ))}
                        </select>
                        <select
                            className={styles.select}
                            value={selectedColumn}
                            onChange={(e) => setSelectedColumn(e.target.value)}
                        >
                            <option value="">Column</option>
                            {['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'].map((column) => (
                                <option key={column} value={column}>
                                    {column}
                                </option>
                            ))}
                        </select>

                        <button className={styles.button} onClick={makeMove}>
                            Make move
                        </button>
                    </div>
                    <div className={styles.results}>
                        <div className={styles.result}>Game ID: {game.id}</div>
                        <div className={styles.result}>Turns: {turns}</div>
                        <div className={styles.result}>Result: {result}</div>
                        <div className={styles.result}>
                            Ships sunken: {shipsSunkenCount}
                        </div>
                    </div>
                    {error && <div className={styles.error}>Error: {error}</div>}
                </div>
            )}
            <BattleshipGrid game={game}/>
            {gameStatus === 'gameOver' && <h2 className={styles.gameOver}>GAME OVER</h2>}
        </div>
    );
};

export default BattleshipGame;
