
import os

# Specify the folder path
folder_path = "D:/funandgiggels/Project-Aurora/Aurora"

# Specify the output file path
output_file = "cs_files_content.txt"

# Open the output file in write mode
with open(output_file, "w") as file:
    # Iterate through all files and subfolders recursively
    for root, dirs, files in os.walk(folder_path):
        for filename in files:
            # Check if the file has a .cs extension
            if filename.endswith(".cs"):
                file_path = os.path.join(root, filename)
                
                # Open the .cs file in read mode
                with open(file_path, "r") as cs_file:
                    # Read the content of the .cs file
                    content = cs_file.read()
                    
                    # Write the filename and content to the output file
                    file.write(f"Filename: {filename}\n")
                    file.write(f"Path: {file_path}\n")
                    file.write(f"Content:\n{content}\n")
                    file.write("-" * 50 + "\n")

print(f"Content of .cs files saved to {output_file}")