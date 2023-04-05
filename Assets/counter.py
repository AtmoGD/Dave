import os

directory = os.getcwd()
folder_path = os.path.join(directory, 'Scripts')

# Initialize counters
file_count = 0
total_line_count = 0
total_word_count = 0

# Recursively walk through the directories and subdirectories
for root, _, files in os.walk(folder_path):
    for filename in files:
        # Check if the file has a .cs extension
        if filename.endswith('.cs'):
            file_count += 1
            file_path = os.path.join(root, filename)

            with open(file_path, 'r', encoding='utf-8') as file:
                lines = file.readlines()
                line_count = len(lines)
                total_line_count += line_count

                for line in lines:
                    words = line.split()
                    word_count = len(words)
                    total_word_count += word_count

print(f"Total .cs files: {file_count}")
print(f"Total line count: {total_line_count}")
print(f"Total word count: {total_word_count}")