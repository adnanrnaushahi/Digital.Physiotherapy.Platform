import React, { useState, useEffect } from 'react';
import {
    Paper,
    Typography,
    List,
    ListItem,
    ListItemText,
    Box,
    CircularProgress,
    Chip,
    Divider,
    Grid,
    LinearProgress,
    Card,
    CardContent
} from '@mui/material';
import { SessionResult } from '../models/SessionResult';
import { getSessionResults } from '../api/api';

const ResultsList: React.FC = () => {
    const [results, setResults] = useState<SessionResult[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadData = async () => {
            try {
                setLoading(true);
                setError(null);
                const resultsData = await getSessionResults();
                setResults(resultsData);
            } catch (err) {
                console.error('Error loading results:', err);
                setError('Failed to load session results. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, []);

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" m={4}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Paper sx={{ p: 2, m: 2 }}>
            <Typography variant="h5" gutterBottom>
                Session Results
            </Typography>

            {error && (
                <Typography color="error" sx={{ mb: 2 }}>
                    {error}
                </Typography>
            )}

            {results.length > 0 ? (
                <List>
                    {results.map((result) => (
                        <Card key={result.id} sx={{ mb: 2 }}>
                            <CardContent>
                                <Box display="flex" justifyContent="space-between" alignItems="center">
                                    <Typography variant="h6">
                                        {result.patientName}
                                    </Typography>
                                    <Typography variant="subtitle2" color="text.secondary">
                                        {new Date(result.sessionDate).toLocaleDateString()}
                                    </Typography>
                                </Box>

                                <Divider sx={{ my: 1.5 }} />

                                {result.notes && (
                                    <>
                                        <Typography variant="body2" sx={{ mt: 1.5 }}>
                                            Notes:
                                        </Typography>
                                        <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                                            {result.notes}
                                        </Typography>
                                    </>
                                )}
                            </CardContent>
                        </Card>
                    ))}
                </List>
            ) : (
                <Typography variant="body1" sx={{ textAlign: 'center', py: 3 }}>
                    No session results found.
                </Typography>
            )}
        </Paper>
    );
};

export default ResultsList;