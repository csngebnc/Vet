export interface AddAppointmentDto {
    startDate: Date,
    endDate: Date,
    treatmentId: number,
    doctorId: number,
    animalId: number
}