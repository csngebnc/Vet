import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { VaccineDto } from 'src/app/_models/VaccineDto';
import { VaccineService } from 'src/app/_services/vaccine.service';

@Component({
  selector: 'app-add-vaccine-record',
  templateUrl: './add-vaccine-record.component.html',
  styleUrls: ['./add-vaccine-record.component.css']
})
export class AddVaccineRecordComponent implements OnInit {

  @Input() animalId;
  vaccines: VaccineDto[] = [];

  constructor(private vaccineService: VaccineService, private fb: FormBuilder, private ngbModal: NgbActiveModal, public datepipe: DatePipe) { }
  addVaccineRecordForm: FormGroup;
  maxDate: Date = new Date();
  filteredVaccines: Observable<VaccineDto[]>;

  ngOnInit(): void {
    this.addVaccineRecordForm = this.fb.group({
      animalId: [this.animalId, Validators.required],
      vaccineId: ['', Validators.required],
      date: ['', Validators.required]
    })

    this.vaccineService.getVaccines().subscribe((vaccines: VaccineDto[]) => {
      this.vaccines = vaccines;
      this.filteredVaccines = this.addVaccineRecordForm.controls['vaccineId'].valueChanges
        .pipe(
          startWith(''),
          map(value => this._filter(value))
        );
    })


  }

  private _filter(value) {
    return this.vaccines.filter(option => option.name.toLowerCase().includes(value.toLowerCase()));
  }

  addVaccineRecord() {
    this.addVaccineRecordForm.patchValue({ date: this.datepipe.transform(this.addVaccineRecordForm.get('date').value, 'yyyy-MM-dd') })
    this.vaccineService.addVaccineRecord(this.addVaccineRecordForm.value).subscribe((res: boolean) => {
      if (res) {
        this.ngbModal.close();
      } else {
        alert('Hiba');
      }
    })
  }
}
