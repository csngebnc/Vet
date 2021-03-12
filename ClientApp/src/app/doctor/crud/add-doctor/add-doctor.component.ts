import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-add-doctor',
  templateUrl: './add-doctor.component.html',
  styleUrls: ['./add-doctor.component.css']
})
export class AddDoctorComponent implements OnInit {

  promoteDoctorForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private doctorService: DoctorService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.promoteDoctorForm = this.fb.group({
      email: ['', Validators.required],
    })
  }

  promoteDoctor(){
    this.doctorService.promoteDoctor(this.promoteDoctorForm.get('email').value).subscribe(() => {
      this.ngbModal.close();
    })
  }

}
