import React from 'react';
import { Container, Typography, Box } from '@mui/material';
import ResultsList from '../components/ResultsList';

const ResultsPage: React.FC = () => {
    return (
        <Container>
            <Box my={4}>
                <Typography variant="h4" component="h1" gutterBottom>
                    Physiotherapy Session Results
                </Typography>
                <Typography variant="body1" color="text.secondary" paragraph>
                    Review patient progress and session outcomes
                </Typography>
                <ResultsList />
            </Box>
        </Container>
    );
};

export default ResultsPage;