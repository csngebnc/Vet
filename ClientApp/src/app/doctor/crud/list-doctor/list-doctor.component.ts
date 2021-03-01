import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorDto } from 'src/app/_models/doctordto';
import { DoctorService } from 'src/app/_services/doctor.service';
import { AddDoctorComponent } from '../add-doctor/add-doctor.component';

@Component({
  selector: 'app-list-doctor',
  templateUrl: './list-doctor.component.html',
  styleUrls: ['./list-doctor.component.css']
})
export class ListDoctorComponent implements OnInit {

  doctors: DoctorDto[] = [];

  constructor(private doctorService: DoctorService, private modalService: NgbModal) {
    this.refreshDoctors();
   }

  ngOnInit(): void {
    
  }

  open() {
    const modalRef = this.modalService.open(AddDoctorComponent);
    modalRef.result.then(() => this.refreshDoctors())
  }


  refreshDoctors(){
    this.doctorService.getDoctors().subscribe((doctors: DoctorDto[]) => {
      this.doctors = doctors;
    })
  }

  demoteDoctor(id){
    this.doctorService.demoteDoctor(id).subscribe(() => {this.refreshDoctors()});
  }

}