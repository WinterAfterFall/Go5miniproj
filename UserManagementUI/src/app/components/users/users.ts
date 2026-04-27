import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';

declare var bootstrap: any;

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css'
})
export class Users implements OnInit {
  users: any[] = [];
  userForm: FormGroup;
  private apiUrl = 'https://localhost:7109/api/Users'; 

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.userForm = this.fb.group({
      userId: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mobileNo: ['', Validators.required],
      roleType: ['Employee', Validators.required],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.fetchUsers();
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    return password && confirmPassword && password.value !== confirmPassword.value 
      ? { passwordMismatch: true } : null;
  }

  fetchUsers(): void {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (res) => this.users = res,
      error: (err) => {
        console.error('ดึงข้อมูลไม่ได้:', err);
        this.users = [];
      }
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const { confirmPassword, ...userData } = this.userForm.value;

      this.http.post(this.apiUrl, userData).subscribe({
        next: (res) => {
          alert('บันทึกข้อมูลสำเร็จ!');
          this.fetchUsers();
          this.userForm.reset({ roleType: 'Employee' });
          
          const closeBtn = document.querySelector('#addUserModal .btn-close') as HTMLElement;
          if (closeBtn) closeBtn.click();
        },
        error: (err) => {
          console.error('บันทึกไม่สำเร็จ:', err);
          alert('เกิดข้อผิดพลาด: ' + (err.error?.message || 'ไม่สามารถเชื่อมต่อ Server ได้'));
        }
      });
    } else {
      this.userForm.markAllAsTouched();
    }
  }

  deleteUser(id: any): void {
    if (confirm('คุณแน่ใจใช่ไหมว่าจะลบ User นี้?')) {
      this.http.delete(`${this.apiUrl}/${id}`).subscribe({
        next: () => {
          alert('ลบข้อมูลสำเร็จ!');
          this.fetchUsers();
        },
        error: (err) => {
          console.error('ลบไม่สำเร็จ:', err);
          alert('เกิดข้อผิดพลาดในการลบข้อมูล');
        }
      });
    }
  }
  
}