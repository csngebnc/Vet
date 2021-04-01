import { TherapiaOnMedicalRecord } from "./therapiarecorddto";

export interface UpdateMedicalRecordDto {
    id: number;
    date: Date;

    ownerEmail: string;

    animalId: number | null;

    anamnesis: string;
    symptoma: string;
    details: string;

    therapias: TherapiaOnMedicalRecord[];

}