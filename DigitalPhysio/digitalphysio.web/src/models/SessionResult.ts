export interface SessionResult {
    id: number;
    prescriptionId: number;
    patientName: string;
    sessionDate: string;
    notes: string;
    exerciseCompletion: Record<string, number>;
}

export interface SessionResultCreate {
    prescriptionId: number;
    sessionDate: string;
    notes: string;
    exerciseCompletion: Record<string, number>;
}