namespace Data.Models;

public  class RepositoryResult<TResult>
{
    public bool Succeeded { get; set; }

    public int StatusCode { get; set; }

    public string? Error {  get; set; }
    
    public TResult? Result { get; set; }

}
