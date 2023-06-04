using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Shared.Domain;

/// <summary>
/// Class to create IFormFile instance.
/// </summary>
public class UrlFileForm : IFormFile
{
	private readonly FileModel fileModel;

	/// <summary>
	/// Initializes a new instance of the <see cref="UrlFileForm"/> class.
	/// </summary>
	/// <param name="fileModel">File model.</param>
	public UrlFileForm(FileModel fileModel)
	{
		this.fileModel = fileModel;
	}

	/// <inheritdoc />
	public string ContentType => "application/octet-stream";

	/// <inheritdoc />
	public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{this.fileModel.FileName}\"";

	/// <inheritdoc />
	public IHeaderDictionary? Headers { get; }

	/// <inheritdoc />
	public long Length => this.fileModel.Content!.Length;

	/// <inheritdoc />
	public string Name => "file";

	/// <inheritdoc />
	public string FileName => this.fileModel.FileName!;

	/// <inheritdoc />
	public Stream OpenReadStream()
	{
		return this.fileModel.Content!;
	}

	/// <inheritdoc />
	public void CopyTo(Stream target)
	{
		this.fileModel.Content!.CopyTo(target);
	}

	/// <inheritdoc />
	public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
	{
		return this.fileModel.Content!.CopyToAsync(target, cancellationToken);
	}
}