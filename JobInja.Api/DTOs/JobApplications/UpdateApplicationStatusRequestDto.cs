using System.ComponentModel.DataAnnotations;
using JobInja.Api.Entities;

namespace JobInja.Api.DTOs.JobApplications;

public class UpdateApplicationStatusRequestDto
{
    [EnumDataType(typeof(ApplicationStatus), ErrorMessage = "وضعیت نامعتبر است")]
    public ApplicationStatus Status { get; set; }
}