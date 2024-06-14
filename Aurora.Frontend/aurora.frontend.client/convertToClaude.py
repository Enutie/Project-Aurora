import os

# Specify the folder path where the script is located
folder_path = os.path.dirname(os.path.abspath(__file__))

# Specify the output file path
output_file = os.path.join(folder_path, "vue_js_files_content.txt")

# Open the output file in write mode
with open(output_file, "w", encoding='utf-8') as file:
    # Iterate through all files and subfolders recursively
    for root, dirs, files in os.walk(folder_path):
        # Exclude node_modules directory
        dirs[:] = [d for d in dirs if d != 'node_modules']
        for filename in files:
            # Check if the file has a .vue or .js extension
            if filename.endswith(".vue") or filename.endswith(".js"):
                file_path = os.path.join(root, filename)
                
                try:
                    # Open the .vue or .js file in read mode with utf-8 encoding
                    with open(file_path, "r", encoding='utf-8') as vue_js_file:
                        # Read the content of the .vue or .js file
                        content = vue_js_file.read()
                        
                        # Write the filename and content to the output file
                        file.write(f"Filename: {filename}\n")
                        file.write(f"Path: {file_path}\n")
                        file.write(f"Content:\n{content}\n")
                        file.write("-" * 50 + "\n")
                except UnicodeDecodeError:
                    print(f"Could not read file: {file_path} due to encoding issues")

print(f"Content of .vue and .js files saved to {output_file}")
